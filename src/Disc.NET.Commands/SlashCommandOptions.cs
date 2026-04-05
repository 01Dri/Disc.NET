using Disc.NET.Commands.Enums;

namespace Disc.NET.Commands
{
    public class SlashCommandData
    {
        public InteractionType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }

}
