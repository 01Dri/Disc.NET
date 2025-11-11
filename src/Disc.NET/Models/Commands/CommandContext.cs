using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Disc.NET.Models.Commands
{
    public class CommandContext : IContext
    {
        public Author? Author { get; set; }

        public Channel? Channel { get; set; }

        public Message? Message { get; set; }


    }
}
