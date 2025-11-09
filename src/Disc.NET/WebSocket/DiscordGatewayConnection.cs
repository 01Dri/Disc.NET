using System.Net.WebSockets;
using System.Text.Json;
using Disc.NET.Configurations;
using Disc.NET.Enums;
using Disc.NET.Extensions;
using Disc.NET.Handlers;
using Microsoft.Extensions.Logging;

namespace Disc.NET.WebSocket
{
    internal class DiscordGatewayConnection
    {
        private readonly byte[] _buffer = new byte[4096];
        private volatile int _lastSequenceEventNumber;
        private readonly ClientWebSocket _ws = new();
        private readonly ILogger<DiscordGatewayConnection> _logger;

        public DiscordGatewayConnection(ILogger<DiscordGatewayConnection> logger)
        {
            _logger = logger;
        }

        public async Task ConnectAsync(string token, AppOptions options)
        {
            _logger.LogInformation("Connecting to Discord Gateway...");

            await _ws.ConnectAsync(new Uri("wss://gateway.discord.gg/?v=10&encoding=json"), CancellationToken.None);
            _logger.LogInformation("Connected successfully.");

            var helloResult = await _ws.ReceiveAsync(_buffer, CancellationToken.None);
            var helloJson = helloResult.GetJsonDocument(_buffer);
            var heartbeatInterval = helloJson.GetHeartbeatInterval();
            _logger.LogInformation("Received HELLO. Heartbeat interval: {Interval} ms", heartbeatInterval);

            _ = Task.Run(async () =>
            {
                _logger.LogInformation("Heartbeat background task started.");

                while (_ws.State == WebSocketState.Open)
                {
                    var heartbeatToSend = JsonSerializer.Serialize(new
                    {
                        op = DiscordWebSocketOpCodesType.Heartbeat,
                        d = _lastSequenceEventNumber == 0 ? (int?)null : _lastSequenceEventNumber
                    });

                    try
                    {
                        await _ws.SendAsync(heartbeatToSend.ToUTF8Bytes(), WebSocketMessageType.Text, true, CancellationToken.None);
                        _logger.LogDebug("Heartbeat sent (seq: {Seq})", _lastSequenceEventNumber);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send heartbeat.");
                    }

                    await Task.Delay(heartbeatInterval);
                }

                _logger.LogWarning("Heartbeat task stopped (WebSocket closed).");
            });

            _logger.LogInformation("Sending Identify payload...");
            await SendIdentifyPayloadAsync(token, options);
            _logger.LogInformation("Identify payload sent.");

            // Event loop
            _logger.LogInformation("Starting event loop...");
            while (_ws.State == WebSocketState.Open)
            {
                var message = await ReadMessageAsync();
                if (message == null)
                    continue;

                var eventName = message.GetEventName();
                if (string.IsNullOrEmpty(eventName))
                {
                    _logger.LogDebug("Received message without event name.");
                    continue;
                }

                var eventType = eventName.ToDiscordWebSocketEventType();
                if (eventType == DiscordWebSocketEventType.None)
                {
                    _logger.LogDebug("Ignoring unsupported event: {Event}", eventName);
                    continue;
                }

                if (eventType == DiscordWebSocketEventType.Ready)
                {
                    _logger.LogInformation("Bot is connected and ready!");
                }

                _logger.LogDebug("Dispatching event: {Event}", eventName);
                await HandlerFactory.CreateHandlerChain().HandleAsync(eventType, "event context", options);
            }

            _logger.LogWarning("Event loop terminated (WebSocket closed).");
        }

        private async Task SendIdentifyPayloadAsync(string token, AppOptions options)
        {
            var identify = JsonSerializer.Serialize(new
            {
                op = DiscordWebSocketOpCodesType.Identify,
                d = new
                {
                    token = $"Bot {token}",
                    intents = options.GatewayIntentsTypes.GetIntIntents(),
                    properties = new
                    {
                        os = "windows",
                        browser = "custom",
                        device = "custom"
                    }
                }
            });

            await _ws.SendAsync(identify.ToUTF8Bytes(), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<JsonDocument?> ReadMessageAsync()
        {
            var message = new List<byte>();
            WebSocketReceiveResult result;

            try
            {
                do
                {
                    result = await _ws.ReceiveAsync(_buffer, CancellationToken.None);
                    message.AddRange(_buffer.Take(result.Count));
                } while (!result.EndOfMessage);
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
            _logger.LogTrace("Received raw message: {Message}", text);

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
