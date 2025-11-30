using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Shared.Configurations;

namespace Disc.NET.Client.SDK
{
    public sealed class ClientSingleton
    {
        private static IClient? _instance;

        public static IClient GetInstance(AppConfiguration appConfiguration)
        {
            if (_instance == null)
                _instance = new Client(appConfiguration, new HttpClient());

            return _instance;
        }

    }
}
