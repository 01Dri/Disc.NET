using Disc.NET.Configurations;
using Disc.NET.WebSocket;

namespace Disc.NET
{
    public class App
    {
        public async Task RunAsync(string token, AppOptions? options = null)
        {
            var discordWebSocketConnection = new DiscordGatewayConnection();
            await discordWebSocketConnection.ConnectAsync(token, options ?? new AppOptions());
        }
    }
}
