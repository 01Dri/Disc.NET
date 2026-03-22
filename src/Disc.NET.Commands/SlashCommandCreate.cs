using Disc.NET.Commands.Enums;

namespace Disc.NET.Commands
{
    public class SlashCommandCreate
    {
        public string Name { get; set; } = string.Empty;
        public InteractionType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<SlashCommandOptions> Options { get; set; } = [];
    }
}
