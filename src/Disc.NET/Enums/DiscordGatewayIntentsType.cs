namespace Disc.NET.Enums
{
    public enum DiscordGatewayIntentsType
    {
        GUILDS = 1 << 0,  // 1
        GUILD_MEMBERS = 1 << 1,  // 2
        GUILD_BANS = 1 << 2,  // 4
        GUILD_EMOJIS_AND_STICKERS = 1 << 3,  // 8
        GUILD_INTEGRATIONS = 1 << 4,  // 16
        GUILD_WEBHOOKS = 1 << 5,  // 32
        GUILD_INVITES = 1 << 6,  // 64
        GUILD_VOICE_STATES = 1 << 7,  // 128
        GUILD_PRESENCES = 1 << 8,  // 256
        GUILD_MESSAGES = 1 << 9,  // 512
        GUILD_MESSAGE_REACTIONS = 1 << 10, // 1024
        GUILD_MESSAGE_TYPING = 1 << 11, // 2048
        DIRECT_MESSAGES = 1 << 12, // 4096
        DIRECT_MESSAGE_REACTIONS = 1 << 13, // 8192
        DIRECT_MESSAGE_TYPING = 1 << 14, // 16384

    }
}
