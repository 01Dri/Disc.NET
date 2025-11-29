using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBot
{
    [SlashCommand("comando_sem_parametros", InteractionType.SubCommand, "teste comand2")]

    internal class SlashCommandTest2 : CommandBase, ISlashCommand
    {
        public async Task<bool> RunAsync(InteractionContext context, SlashCommandParamsResult result)
        {
            return true;
        }

    }
}
