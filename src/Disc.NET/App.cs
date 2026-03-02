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
            var gateway = new GatewayConnection(configuration,_logger);
            await gateway.ConnectAsync();
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
        /// <summary>
        /// Configures the container usage strategy for the application.
        ///
        /// By default, object creation is performed using the standard C# <see cref="Activator"/>,
        /// which does not provide support for dependency injection.
        ///
        /// When this method is called, the application switches to using the Autofac container,
        /// enabling full dependency injection support for registered components.
        ///
        /// In summary:
        /// - Without calling this method → uses default Activator (no DI)
        /// - Calling this method → enables Autofac and dependency injection
        /// </summary>
        /// <param name="appConfiguration">
        /// Application configuration instance where the container selection flag is stored.
        /// </param>
        /// <returns>
        /// Returns the singleton instance of <see cref="DiscNetContainer"/> configured to use Autofac.
        /// </returns>
        public DiscNetContainer UseDependencyInjection(AppConfiguration appConfiguration)
        {
            appConfiguration.UseContainer = true;
            return DiscNetContainer.GetInstance();
        }

    }

}
