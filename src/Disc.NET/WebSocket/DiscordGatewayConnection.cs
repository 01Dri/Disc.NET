using System.Net.WebSockets;
using System.Text.Json;
using Disc.NET.Configurations;
using Disc.NET.Enums;
using Disc.NET.Extensions;
using Disc.NET.Handlers;

namespace Disc.NET.WebSocket
{
    /// <summary>
    /// Manages a persistent WebSocket connection with the Discord Gateway.
    /// </summary>
    /// <remarks>
    /// This class handles all low-level operations required to connect, authenticate,
    /// maintain heartbeats, and receive events from Discord's real-time gateway.
    /// 
    /// <para>
    /// For more information, see:
    /// <see href="https://discord.com/developers/docs/topics/gateway">Discord Gateway Documentation</see>.
    /// </para>
    /// </remarks>
    internal class DiscordGatewayConnection
    {
        private readonly byte[] _buffer = new byte[4096];
        private volatile int _lastSequenceEventNumber;
        private readonly ClientWebSocket _ws = new();

        /// <summary>
        /// Establishes a WebSocket connection with the Discord Gateway and starts the event loop.
        /// </summary>
        /// <remarks>
        /// Performs the following steps:
        /// <list type="number">
        /// <item>Connects to the Discord Gateway (<c>wss://gateway.discord.gg/?v=10&encoding=json</c>).</item>
        /// <item>Receives the initial <c>HELLO</c> payload and extracts the heartbeat interval.</item>
        /// <item>Starts a background heartbeat task that periodically sends <c>OP 1</c> payloads.</item>
        /// <item>Sends the Identify payload to authenticate with the provided bot token.</item>
        /// <item>Begins reading and dispatching incoming Gateway messages.</item>
        /// </list>
        /// </remarks>
        /// <param name="token">The bot token used to authenticate with the Discord Gateway.</param>
        /// <param name="options">Application configuration options containing gateway intents and other metadata.</param>
        /// <returns>A task that represents the asynchronous connection operation.</returns>
        /// <example>
        /// <code>
        /// var gateway = new DiscordGatewayConnection();
        /// await gateway.ConnectAsync("YOUR_TOKEN_HERE", new AppOptions());
        /// </code>
        /// </example>
        public async Task ConnectAsync(string token, AppOptions options)
        {
            await _ws.ConnectAsync(new Uri("wss://gateway.discord.gg/?v=10&encoding=json"), CancellationToken.None);

            var helloResult = await _ws.ReceiveAsync(_buffer, CancellationToken.None);
            var helloJson = helloResult.GetJsonDocument(_buffer);
            var heartbeatInterval = helloJson.GetHeartbeatInterval();

            // Start background heartbeat task
            _ = Task.Run(async () =>
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var heartbeatToSend = JsonSerializer.Serialize(new
                    {
                        op = DiscordWebSocketOpCodesType.Heartbeat,
                        d = _lastSequenceEventNumber == 0 ? (int?)null : _lastSequenceEventNumber
                    });

                    await _ws.SendAsync(heartbeatToSend.ToUTF8Bytes(), WebSocketMessageType.Text, true, CancellationToken.None);
                    Console.WriteLine("Heartbeat sent: " + heartbeatToSend);

                    await Task.Delay(heartbeatInterval);
                }
            });

            await SendIdentifyPayloadAsync(token, options);

            // Event loop
            while (_ws.State == WebSocketState.Open)
            {
                var message = await ReadMessageAsync();
                if (message == null)
                    continue;

                var eventName = message.GetEventName();
                if (string.IsNullOrEmpty(eventName))
                    continue;

                // Dispatch event to registered handlers
                await HandlerExecutor.ExecuteHandlerAsync(eventName.ToDiscordWebSocketEventsType(), "event context data", options);
            }
        }

        /// <summary>
        /// Sends the Identify payload to the Discord Gateway, authenticating the client session.
        /// </summary>
        /// <param name="token">The bot token.</param>
        /// <param name="options">Application configuration options, used to include the gateway intents.</param>
        /// <returns>A task representing the asynchronous send operation.</returns>
        /// <remarks>
        /// The Identify payload is required immediately after the initial <c>HELLO</c> event.
        /// This authenticates the client and tells Discord which events it wishes to receive
        /// (based on the provided intents).
        /// </remarks>
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

        /// <summary>
        /// Reads the next full message from the WebSocket stream and parses it into a <see cref="JsonDocument"/>.
        /// </summary>
        /// <remarks>
        /// This method handles message fragmentation automatically.  
        /// It also updates the internal sequence number (<c>_lastSequenceEventNumber</c>)
        /// whenever a message includes a valid <c>s</c> field from Discord.
        /// </remarks>
        /// <returns>
        /// A <see cref="JsonDocument"/> representing the received payload, 
        /// or <see langword="null"/> if the message is empty.
        /// </returns>
        private async Task<JsonDocument?> ReadMessageAsync()
        {
            var message = new List<byte>();
            WebSocketReceiveResult result;

            do
            {
                result = await _ws.ReceiveAsync(_buffer, CancellationToken.None);
                message.AddRange(_buffer.Take(result.Count));
            } while (!result.EndOfMessage);

            if (message.Count <= 0)
                return null;

            var text = message.ToArray().ToUTF8String();
            var json = JsonDocument.Parse(text);

            var newLastSequenceEventNumber = json.GetLastSequenceEventNumber();
            _lastSequenceEventNumber = newLastSequenceEventNumber == 0
                ? _lastSequenceEventNumber
                : newLastSequenceEventNumber;

            return json;
        }
    }
}
