using System.Text.Json.Serialization;
using Disc.NET.Client.SDK.Messages.Embeds;
using Disc.NET.Shared.Enums;

namespace Disc.NET.Client.SDK.Messages
{
    public class ApiMessage
    {
        public string Content { get; set; } = string.Empty;
        public List<Embed> Embeds { get; set; } = [];

        [JsonIgnore]
        public List<MessageFlag>? MessageFlags { get; set; } 

        public long Flags => MessageFlags?.Aggregate(0L, (current, flag) => current | (long)flag) ?? 0L;


        // Colocar os outros campos depois
        // https://discord.com/developers/docs/resources/message#message-object
    }

}
