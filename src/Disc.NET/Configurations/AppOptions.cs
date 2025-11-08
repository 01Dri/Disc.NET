using Disc.NET.Enums;

namespace Disc.NET.Configurations
{
    public class AppOptions
    {
        public char BotPrefix { get; init;  }
        public List<DiscordGatewayIntentsType> GatewayIntentsTypes { get; init; } = new()
        {
            DiscordGatewayIntentsType.GUILD_MESSAGES,
            DiscordGatewayIntentsType.DIRECT_MESSAGES
        };
        public AppOptions(char botPrefix = '!')
        {
            BotPrefix = botPrefix;
        }
    }
}
