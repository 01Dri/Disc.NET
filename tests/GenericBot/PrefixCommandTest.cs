using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Attributes.Commands;
using Disc.NET.Commands;

namespace GenericBot
{
    [PrefixCommand("helloword")]
    public class PrefixCommandTest : IDiscordCommand
    {
        public async Task<bool> RunAsync()
        {
            Console.WriteLine("Hello World!");
            return true;
        }
    }
}
