using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;

namespace GenericBot
{
    [PrefixCommand("helloword")]
    public class PrefixCommandTest : ICommand<CommandContext>
    {
        public async Task<bool> RunAsync(CommandContext context)
        {
            Console.WriteLine("Command by " + context.Author?.Username);
            Console.WriteLine("Channel " + context.Channel?.Id);
            Console.WriteLine("Content" + context.Message?.Content);

            return true;
        }
    }
}
