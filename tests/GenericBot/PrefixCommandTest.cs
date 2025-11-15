using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Shared.Configurations;

namespace GenericBot
{
    [PrefixCommand("helloword")]
    public class PrefixCommandTest : CommandBase, ICommand<CommandContext>
    {

        public async Task<bool> RunAsync(CommandContext context)
        {
            await UseClient().SendMessageAsync(context.Channel?.Id!,new ApiMessage()
            {
                Content = "Olá Mundo",
                Embeds = 
                [
                    new()
                    {
                        Title = "Título do Embed",
                        Description = "Descrição do Embed"
                    }
                ]
            });
            return true;
        }

    }

}
