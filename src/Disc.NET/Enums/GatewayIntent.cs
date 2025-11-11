namespace Disc.NET.Enums
{
    /// <summary>
    /// Represents all available Gateway Intents used to specify which events the bot should receive from Discord.
    /// Docs: https://discord.com/developers/docs/topics/gateway#gateway-intents
    /// </summary>
    [Flags]
    public enum GatewayIntent
    {
        // ---- GUILD-RELATED ----
        /// <summary> Guild creation, update, deletion, and role updates </summary>
        GUILDS = 1 << 0, // 1

        /// <summary> Guild member add, update, remove (Privileged Intent) </summary>
        GUILD_MEMBERS = 1 << 1, // 2

        /// <summary> Ban add and remove events </summary>
        GUILD_BANS = 1 << 2, // 4

        /// <summary> Emojis and stickers updates </summary>
        GUILD_EMOJIS_AND_STICKERS = 1 << 3, // 8

        /// <summary> Integrations (like Twitch/YouTube) updates </summary>
        GUILD_INTEGRATIONS = 1 << 4, // 16

        /// <summary> Webhook updates </summary>
        GUILD_WEBHOOKS = 1 << 5, // 32

        /// <summary> Invite creation and deletion </summary>
        GUILD_INVITES = 1 << 6, // 64

        /// <summary> Voice state changes (join/leave/mute/deafen) </summary>
        GUILD_VOICE_STATES = 1 << 7, // 128

        /// <summary> Presence updates (Privileged Intent) </summary>
        GUILD_PRESENCES = 1 << 8, // 256

        /// <summary> Messages sent in guild text channels </summary>
        GUILD_MESSAGES = 1 << 9, // 512

        /// <summary> Reaction add/remove events in guild channels </summary>
        GUILD_MESSAGE_REACTIONS = 1 << 10, // 1024

        /// <summary> Typing start events in guild channels </summary>
        GUILD_MESSAGE_TYPING = 1 << 11, // 2048

        // ---- DIRECT MESSAGE RELATED ----
        /// <summary> Messages sent in direct messages </summary>
        DIRECT_MESSAGES = 1 << 12, // 4096

        /// <summary> Reaction add/remove events in DMs </summary>
        DIRECT_MESSAGE_REACTIONS = 1 << 13, // 8192

        /// <summary> Typing start events in DMs </summary>
        DIRECT_MESSAGE_TYPING = 1 << 14, // 16384

        // ---- GUILD MESSAGE CONTENT ----
        /// <summary> Access to message content (Privileged Intent, required for non-slash bots) </summary>
        MESSAGE_CONTENT = 1 << 15, // 32768

        // ---- NEWER INTENTS (Discord API v10+) ----
        /// <summary> Scheduled events updates (creation, update, delete, user join/leave)</summary>
        GUILD_SCHEDULED_EVENTS = 1 << 16, // 65536

        /// <summary> Auto moderation rule create/update/delete events </summary>
        AUTO_MODERATION_CONFIGURATION = 1 << 20, // 1048576

        /// <summary> Auto moderation execution events </summary>
        AUTO_MODERATION_EXECUTION = 1 << 21, // 2097152

        /// <summary> Guild message polls (introduced in Discord 2024 update) </summary>
        GUILD_MESSAGE_POLLS = 1 << 24, // 16777216

        /// <summary> Direct message polls (introduced in Discord 2024 update) </summary>
        DIRECT_MESSAGE_POLLS = 1 << 25, // 33554432
    }
}
