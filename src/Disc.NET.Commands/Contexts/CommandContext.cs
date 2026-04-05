using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Commands.Responses;

namespace Disc.NET.Commands.Contexts
{
    public class CommandContext : ContextBase
    {
        public string Id { get; set; } = string.Empty;
        public Author? Author { get; set; }

        public int ChannelType { get; set; }
        public string ChannelId { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;

        public DateTime? Timestamp { get; set; }
        public int Type { get; set; }
        public DateTime? EditedTimestamp { get; set; }

        public IResponse Response { get; set; }
    }
}

