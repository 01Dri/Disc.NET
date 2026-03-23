using Disc.NET.Client.SDK.Messages.Components.Enums;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Commands.Responses;

namespace Disc.NET.Commands.Contexts
{
    public class InteractionContext : ContextBase
    {
        public Member? Member { get; set; }
        public string Id { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;
        public Channel? Channel { get; set; }
        public int Type { get; set; }
        public int Context { get; set; }

        public InteractionData? Data { get; set; }
        public InteractionResponse Response { get; set; }
    }

    public class InteractionData
    {
        public List<string> Values { get; set; } = [];
        public int Id { get; set; }

        public string? CustomId { get; set; }

        public MessageComponentType ComponentType { get; set; }
    }


}
