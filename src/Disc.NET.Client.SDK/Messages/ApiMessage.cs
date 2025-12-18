using System.Text.Json.Serialization;
using Disc.NET.Client.SDK.Messages.Components.Builders.ComponentBuilders;
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

        [JsonIgnore]
        public List<IMessageComponentBuilder> Components { get; set; } = [];

        public List<object> GetComponents()
        {
            var objects = new List<object>();
			Components.ForEach(x => objects.Add(x.Build()));
            return objects;
        }

        // Colocar os outros campos depois
        // https://discord.com/developers/docs/resources/message#message-object
    }


}
