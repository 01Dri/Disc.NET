using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Shared.Enums;

namespace Disc.NET.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SlashCommandAttribute : Attribute
    {
        public string Name { get; init; }
        public InteractionType Type { get; init; }
        public string Description { get; init; }

        public string? GuildId { get; set; }

        public SlashCommandAttribute(string name, InteractionType type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }
    }


}
