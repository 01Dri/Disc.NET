using Disc.NET.Configuration;
using Disc.NET.Gateway;

namespace Disc.NET
{
    public class App
    {
        private readonly AppConfiguration _appConfiguration;

        public App(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public async Task RunAsync()
        {
            var gateway = new GatewayConnection(_appConfiguration);
            await gateway.ConnectAsync();
        }
    }

}
