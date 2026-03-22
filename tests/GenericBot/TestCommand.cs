using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Enums;

namespace GenericBot
{

    [SlashCommand("test", InteractionType.SubCommand, "Ver as informações de tempo da sua cidade")]
    public class TestCommand : ISlashCommand
    {
        public async Task RunAsync(InteractionContext context, SlashCommandParamsResult @params)
        {
            var message = new Message<InteractionContext>()
            {
                Content = "Teste"

            };

            await context.Response.SendMessageAsync(message);
        }
    }
}
