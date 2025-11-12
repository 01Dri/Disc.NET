using Disc.NET.Commands.Contexts.Models;

namespace Disc.NET.Commands.Contexts
{
    public class CommandContext : IContext
    {
        public Author? Author { get; set; }

        public Channel? Channel { get; set; }

        public Message? Message { get; set; }


    }
}
