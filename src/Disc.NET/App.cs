using Disc.NET.Configurations;
using Disc.NET.WebSocket;
using Microsoft.Extensions.Logging;

namespace Disc.NET
{
    public class App
    {
        public async Task RunAsync(string token, AppOptions? options = null)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            var logger = loggerFactory.CreateLogger<DiscordGatewayConnection>();
            var gateway = new DiscordGatewayConnection(logger);
            await gateway.ConnectAsync(token, options ?? new AppOptions());
        }
    }
}
