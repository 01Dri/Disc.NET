using Disc.NET.Enums;

namespace Disc.NET.Configurations
{
    public class AppOptions
    {
        public char BotPrefix { get; init;  }
        public List<GatewayIntent> Intents { get; init; } = new()
        {
            GatewayIntent.GUILD_MESSAGES,
            GatewayIntent.DIRECT_MESSAGES
        };
        public AppOptions(char botPrefix = '!')
        {
            BotPrefix = botPrefix;
        }
    }
}
