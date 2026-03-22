using Disc.NET.Commands.Enums;

namespace Disc.NET.Commands
{
    public class SlashCommandOptions
    {
        public required string Name { get; set; }
        public required InteractionType Type { get; set; }
        public string? Description { get; set; }
        public bool Required { get; set; }
        public List<SlashCommandChoices> Choices { get; set; } = [];
    }

    public class SlashCommandChoices
    {
        public required string Name { get; set; }
        public string? Value { get; set; }
    }

    public class SlashCommandParamsResult
    {
        public InteractionType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public List<SlashCommandOptionResult> Options { get; set; } = [];
    }

    public class SlashCommandOptionResult
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public InteractionType Type { get; set; }
    }
}
