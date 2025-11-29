using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands
{
    public interface IPrefixCommand  : ICommand<CommandContext>
    {
        Task<bool> RunAsync(CommandContext context, List<string> @params);
    }
}