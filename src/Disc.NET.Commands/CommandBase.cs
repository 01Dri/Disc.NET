using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Shared.Configurations;

namespace Disc.NET.Commands
{
    public class CommandBase
    {
        private static CommandBase? _instance;
        private readonly IClient _client;


        protected CommandBase()
        {
        }

        private CommandBase(AppConfiguration appConfiguration)
        {
            _client = new Client.SDK.Client(appConfiguration, new HttpClient());
        }

        public static CommandBase GetInstance(AppConfiguration appConfiguration)
        {
            if (_instance == null)
                _instance = new CommandBase(appConfiguration);

            return _instance;
        }

        public IClient UseClient() => _instance!._client;


    }

}
