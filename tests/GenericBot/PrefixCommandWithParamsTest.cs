using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Client.SDK.Messages;

namespace GenericBot
{
    [PrefixCommand("helloword2")]
    public class PrefixCommandWithParamsTest : CommandBase, ICommand<CommandContext> 
    {
        public async Task<bool> RunAsync(CommandContext context, List<string> @params)
        {
            if (@params.Contains("test"))
            {
                await UseClient().SendMessageAsync(context.Channel?.Id!, new ApiMessage()
                {
                    Content = "Parâmetro 'test' recebido!",
                    Embeds =
                    [
                        new()
                        {
                            Title = "Parâmetro Teste",
                            Description = "Você enviou o parâmetro 'test' com o comando."
                        }
                    ]
                });
            }
            return true;
        }
    }
}
