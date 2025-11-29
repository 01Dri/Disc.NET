using Disc.NET.Commands.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Commands.Attributes;

namespace Disc.NET.Commands
{
    public interface ISlashCommand  : ICommand<InteractionContext>
    {
        Task<bool> RunAsync(InteractionContext context, SlashCommandParamsResult paramsResult);
        List<SlashCommandOptions> BuildOptions();

    }
}
