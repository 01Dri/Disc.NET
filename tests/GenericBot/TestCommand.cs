using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.Enums;
using Disc.NET.Commands.MessageBuilders;

namespace GenericBot
{

    [SlashCommand("test", InteractionType.SubCommand, "Ver as informações de tempo da sua cidade")]
    public class TestCommand : ISlashCommand
    {
        public async Task RunAsync(InteractionContext context, SlashCommandParamsResult @params)
        {

            List<ContextBase> teste = new() { new InteractionContext() { Id = "123" } };
            var message = new Message()
            {
                Content = "Teste",
                ActionRows = new List<IActionRowBuilder>
                {
                    new ActionRowButtonBuilder()
                        .AddButton(
                            new ButtonComponent(ButtonStyle.Primary)
                            {
                                CustomId = "test_button",
                                Label = "Testar"
                            },
                            context,
                            TestCallbackAsync
                        )
                }
            };

            await context.Response.SendMessageAsync(message);
        }

        private async Task TestCallbackAsync(InteractionContext context)
        {
            await context.Response.SendMessageAsync(new Message()
            {
                Content = "Teste",
            });
        }
    }
}
