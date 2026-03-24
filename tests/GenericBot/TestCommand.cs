using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Client.SDK.Messages.Components.Selects;
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
        public async Task RunAsync(InteractionContext context, CancellationToken cancellation = default)
        {

            var message = new Message()
            {
                Content = "Teste",
                ActionRows = new List<IActionRowBuilder>
                {
                    new ActionRowBuilder()
                        .AddSelectMenu("test_select", new List<StringSelectOption>()
                        {
                            new StringSelectOption()
                            {
                                Label = "Opção 1",
                                Value = "option_1"
                            },

                        }, context: context, callback: TestCallback3Async),


                    new ActionRowBuilder()
                        .AddButton("Testar", "test_button", ButtonStyle.Primary, context, TestCallbackAsync)
                        .AddLinkButton("Testar 2", "https://www.linkedin.com/in/dridev/")
                }
            };

            await context.Response.SendMessageAsync(message, cancellation);
        }

        private async Task TestCallbackAsync(InteractionContext context)
        {
            await context.Response.SendMessageAsync(new Message()
            {
                Content = "Teste",
                Type = 4,
            });
        }

        private async Task TestCallback3Async(InteractionContext context)
        {
            var teste = new ButtonComponent(ButtonStyle.Danger)
            {
                Label = "Teste 3",
                CustomId = "test_button_3",
            };
            await context.Response.InteractionRespondingAsync(new Message()
            {
                Content = "teste",
                ActionRows = [new ActionRowBuilder().AddButton<InteractionContext>(teste)]
            }, true);
        }
    }
}
