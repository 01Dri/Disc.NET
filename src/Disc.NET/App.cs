using System.ComponentModel;
using Disc.NET.Configurations;
using Disc.NET.Gateway;
using Microsoft.Extensions.Logging;

namespace Disc.NET
{
    public class App
    {
        private ILogger<GatewayConnection> _logger;

        public App()
        {
            _logger = CreateLogger(LogLevel.Information);
        }

        public async Task RunAsync(string token, AppOptions? options = null)
        {
            var gateway = new GatewayConnection(_logger);
            await gateway.ConnectAsync(token, options ?? new AppOptions());
        }

        public App WithDebugLogger()
        {
            _logger = CreateLogger(LogLevel.Debug);
            return this;
        }

        private ILogger<GatewayConnection> CreateLogger(LogLevel level)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(level);
            });

            return loggerFactory.CreateLogger<GatewayConnection>();
        }
    }
}
