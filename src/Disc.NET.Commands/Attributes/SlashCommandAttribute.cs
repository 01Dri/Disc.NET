using Disc.NET.Commands.Enums;

namespace Disc.NET.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SlashCommandAttribute : Attribute
    {
        public string Name { get; init; }
        public InteractionType Type => InteractionType.SubCommand;
        public string Description { get; init; }

        public string? GuildId { get; set; }

        public SlashCommandAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }


}
