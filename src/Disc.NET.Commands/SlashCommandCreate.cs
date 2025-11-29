using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Commands.Attributes;
using Disc.NET.Shared.Enums;

namespace Disc.NET.Commands
{
    public class SlashCommandCreate
    {
        public string Name { get; set; }
        public InteractionType Type { get; set; }
        public string Description { get; set; }
        public List<SlashCommandOptions> Options { get; set; } = [];
    }
}
