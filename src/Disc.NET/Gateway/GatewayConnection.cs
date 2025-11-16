using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Channels;
using Disc.NET.Handlers;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using Disc.NET.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace Disc.NET.Gateway
{
    /// <summary>
    /// Manages a persistent WebSocket connection to the Discord Gateway.
    /// </summary>
    /// <remarks>
    /// This class handles the full lifecycle of a Discord Gateway session, including:
    /// - Establishing a WebSocket connection.
    /// - Handling the HELLO and READY events.
    /// - Sending Identify payloads for authentication.
    /// - Managing the heartbeat loop and acknowledging heartbeats.
    /// - Dispatching received events to their appropriate handlers.
    /// </remarks>
    internal class GatewayConnection
    {
        private readonly byte[] _buffer = new byte[4096];
        private volatile int _lastSequenceEventNumber;
        private volatile bool _heartbeatAckReceived = true;
        private CancellationTokenSource? _heartbeatCts;
        private readonly ClientWebSocket _ws = new();
        private readonly Channel<JsonDocument> _channel = Channel.CreateUnbounded<JsonDocument>();
        private readonly ILogger<GatewayConnection> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatewayConnection"/> class.
        /// </summary>
        /// <param name="logger">Logger instance used for structured logging and diagnostics.</param>
        public GatewayConnection(ILogger<GatewayConnection> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Establishes a connection to the Discord Gateway and starts the event handling loop.
        /// </summary>
        /// <param name="token">The bot token used for authentication.</param>
        /// <param name="configuration">Application configuration including intents and configuration details.</param>
        /// <remarks>
        /// This method:
        /// <list type="bullet">
        /// <item>Connects to the Discord WebSocket endpoint.</item>
        /// <item>Receives the HELLO payload and initializes the heartbeat process.</item>
        /// <item>Sends the Identify payload for authentication.</item>
        /// <item>Starts concurrent loops for reading WebSocket messages and handling them asynchronously.</item>
        /// </list>
        /// </remarks>
        public async Task ConnectAsync(AppConfiguration configuration)
        {
            _logger.LogInformation("Connecting to Discord Gateway...");

            await _ws.ConnectAsync(new Uri("wss://gateway.discord.gg/?v=10&encoding=json"), CancellationToken.None);
            _logger.LogInformation("Connected successfully.");

            // Wait for the initial HELLO event and start the heartbeat loop
            var helloResult = await _ws.ReceiveAsync(_buffer, CancellationToken.None).ConfigureAwait(false);
            var helloJson = helloResult.GetJsonDocument(_buffer);
            var heartbeatInterval = helloJson.GetHeartbeatInterval();
            _logger.LogDebug("Received HELLO. Heartbeat interval: {Interval} ms", heartbeatInterval);
            StartHeartbeatLoop(heartbeatInterval);

            // Identify and authenticate
            _logger.LogDebug("Sending Identify payload...");
            await SendIdentifyPayloadAsync(configuration.Token, configuration).ConfigureAwait(false);
            _logger.LogDebug("Identify payload sent.");

            // Producer: Reads WebSocket messages and writes them to a channel
            _ = Task.Run(async () =>
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var message = await ReadMessageAsync().ConfigureAwait(false);
                    if (message != null)
                        await _channel.Writer.WriteAsync(message);
                }

                _channel.Writer.Complete();
            });

            // Consumer: Reads messages from the channel and dispatches them
            await foreach (var message in _channel.Reader.ReadAllAsync())
            {
                var opCode = (GatewayOpCode)message.GetOpCode();
                if (!Enum.IsDefined(typeof(GatewayOpCode), opCode))
                {
                    _logger.LogDebug("Received message without op code.");
                    continue;
                }

                if (opCode == GatewayOpCode.HeartbeatAck)
                {
                    _logger.LogDebug("Received Heartbeat ACK.");
                    _heartbeatAckReceived = true;
                    continue;
                }

                var eventName = message.GetEventName();
                if (string.IsNullOrEmpty(eventName))
                {
                    _logger.LogDebug("Received message without event name.");
                    continue;
                }

                var eventType = eventName.ToGatewayEventType();
                if (!Enum.IsDefined(eventType))
                {
                    _logger.LogDebug("Ignoring unsupported event: {Event}", eventName);
                    continue;
                }

                if (eventType == GatewayEvent.Ready)
                {
                    _logger.LogInformation("Bot is connected and ready!");
                }

                var eventContextData = message.GetEventContextData();
                _logger.LogDebug("Dispatching event: {Event}", eventName);
                await HandlerFactory.CreateHandlerChain(configuration).HandleAsync(eventType, eventContextData, configuration)
                   .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Starts the background heartbeat loop, periodically sending heartbeats to Discord and waiting for acknowledgments.
        /// </summary>
        /// <param name="heartbeatInterval">The interval (in milliseconds) between heartbeats, as provided by the HELLO payload.</param>
        /// <remarks>
        /// If a heartbeat acknowledgment (ACK) is not received before the next scheduled heartbeat,
        /// the connection is considered unhealthy and should be reestablished.
        /// </remarks>
        private void StartHeartbeatLoop(int heartbeatInterval)
        {
            _heartbeatCts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                _logger.LogDebug("Heartbeat background task started.");

                while (!_heartbeatCts.Token.IsCancellationRequested && _ws.State == WebSocketState.Open)
                {
                    if (!_heartbeatAckReceived)
                    {
                        _logger.LogWarning("Missed heartbeat ACK! Reconnecting...");
                        // TODO: Trigger reconnection logic here
                        return;
                    }

                    _heartbeatAckReceived = false;

                    var heartbeatPayload = JsonSerializer.Serialize(new
                    {
                        op = GatewayOpCode.Heartbeat,
                        d = _lastSequenceEventNumber == 0 ? (int?)null : _lastSequenceEventNumber
                    });

                    try
                    {
                        await _ws.SendAsync(
                            heartbeatPayload.ToUTF8Bytes(),
                            WebSocketMessageType.Text,
                            true,
                            _heartbeatCts.Token
                        ).ConfigureAwait(false);

                        _logger.LogDebug("Heartbeat sent (seq: {Seq})", _lastSequenceEventNumber);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send heartbeat.");
                        break;
                    }

                    try
                    {
                        await Task.Delay(heartbeatInterval, _heartbeatCts.Token).ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }

                _logger.LogWarning("Heartbeat task stopped.");
            }, _heartbeatCts.Token);
        }

        /// <summary>
        /// Sends the Identify payload to the Discord Gateway to authenticate the bot session.
        /// </summary>
        /// <param name="token">The bot authentication token.</param>
        /// <param name="configuration">The application configuration configuration, including intents.</param>
        /// <remarks>
        /// The Identify payload includes OS, device, and browser properties as required by the Discord API.
        /// </remarks>
        private async Task SendIdentifyPayloadAsync(string token, AppConfiguration configuration)
        {
            var identify = JsonSerializer.Serialize(new
            {
                op = GatewayOpCode.Identify,
                d = new
                {
                    token = $"Bot {token}",
                    intents = configuration.Intents.GetIntIntents(),
                    properties = new
                    {
                        os = "windows",
                        browser = "custom",
                        device = "custom"
                    }
                }
            });

            await _ws.SendAsync(
                identify.ToUTF8Bytes(),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads and parses the next message from the Discord WebSocket stream.
        /// </summary>
        /// <returns>
        /// A <see cref="JsonDocument"/> representing the parsed message payload,
        /// or <c>null</c> if the message is invalid or could not be read.
        /// </returns>
        private async Task<JsonDocument?> ReadMessageAsync()
        {
            var message = new List<byte>();
            WebSocketReceiveResult result;

            try
            {
                do
                {
                    result = await _ws.ReceiveAsync(_buffer, CancellationToken.None).ConfigureAwait(false);
                    message.AddRange(_buffer.Take(result.Count));
                }
                while (!result.EndOfMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error receiving message.");
                return null;
            }

            if (message.Count <= 0)
            {
                _logger.LogDebug("Received empty message.");
                return null;
            }

            var text = message.ToArray().ToUTF8String();
            _logger.LogTrace("Received raw message: {RestMessage}", text);

            var json = JsonDocument.Parse(text);
            var newSeq = json.GetLastSequenceEventNumber();

            if (newSeq != 0 && newSeq != _lastSequenceEventNumber)
            {
                _logger.LogDebug("Sequence updated from {OldSeq} to {NewSeq}", _lastSequenceEventNumber, newSeq);
                _lastSequenceEventNumber = newSeq;
            }

            return json;
        }
    }
}
