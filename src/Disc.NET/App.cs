using Disc.NET.Configuration;
using Disc.NET.Gateway;
using Disc.NET.Shared.Configurations;
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

        public async Task RunAsync(AppConfiguration configuration)
        {
            if (configuration.UseContainer)
            {
                DiscNetContainer.GetInstance().RegisterDependencies();
            }
            var gateway = new GatewayConnection(_logger);
            await gateway.ConnectAsync(configuration);
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

        public DiscNetContainer UseDependencyInjection(AppConfiguration appConfiguration)
        {
            appConfiguration.UseContainer = true;
            return DiscNetContainer.GetInstance();
        }

    }

}
