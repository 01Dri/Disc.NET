using Disc.NET.Configurations;
using Disc.NET.Gateway;
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

            var logger = loggerFactory.CreateLogger<GatewayConnection>();
            var gateway = new GatewayConnection(logger);
            await gateway.ConnectAsync(token, options ?? new AppOptions());
        }
    }
}
