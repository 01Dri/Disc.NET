using Disc.NET.Client.SDK.Interfaces;

namespace Disc.NET.Client.SDK
{
    public sealed class ClientSingleton
    {
        private static IClient? _instance;
        private static ClientConfiguration _clientConfiguration;

        public static IClient  GetInstance()
        {
            if (_clientConfiguration == null) 
            {   
                throw new InvalidOperationException("Client configuration is not set. Please call Configure method before getting the instance."); 
            }

            if (_instance == null)
            {
                _instance = new Client(_clientConfiguration, new HttpClient());
            }

            return _instance;
        }

        public static void Configure(string token, long applicationId)
        {
            if (_clientConfiguration == null)
                _clientConfiguration = new ClientConfiguration(token, applicationId);
        }

    }
}
