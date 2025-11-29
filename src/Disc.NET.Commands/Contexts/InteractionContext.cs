using Disc.NET.Commands.Contexts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disc.NET.Commands.Contexts
{
    public class InteractionContext : IContext
    {
        public Member? Member { get; set; }
        public string Id { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;
        public Channel?  Channel { get; set; }
        public int Type { get; set; }
        public int Context { get; set; }
    }
}
