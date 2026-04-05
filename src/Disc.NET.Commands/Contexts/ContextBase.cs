using Disc.NET.Commands.Contexts.Models;

namespace Disc.NET.Commands.Contexts
{
    public abstract class ContextBase
    {
        public string Id { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;
        public Channel? Channel { get; set; }

        public string Token { get; set; } = string.Empty;

    }
}
