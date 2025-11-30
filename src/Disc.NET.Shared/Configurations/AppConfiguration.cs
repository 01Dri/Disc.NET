using Autofac;
using Disc.NET.Shared.Enums;


namespace Disc.NET.Shared.Configurations
{
    public class AppConfiguration
    {
        public  string Token { get; }
        public char BotPrefix { get; init;  }
        public required long ApplicationId { get; init; }
        public bool UseContainer { get; set; }
        public List<GatewayIntent> Intents { get; init; } = new()
        {
            GatewayIntent.GUILD_MESSAGES,
            GatewayIntent.DIRECT_MESSAGES
        };
        public AppConfiguration(string token, char botPrefix = '!')
        {
            BotPrefix = botPrefix;
            Token = token;
        }

    }
}
