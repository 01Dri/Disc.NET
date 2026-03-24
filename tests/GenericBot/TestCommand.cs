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

            List<ContextBase> teste = new() { new InteractionContext() { Id = "123" } };
            var message = new Message()
            {
                Content = "Teste",
                ActionRows = new List<IActionRowBuilder>
                {
                    new ActionRowSelectMenuBuilder()
                    .AddComponent(new StringSelectComponent()
                    {
                        CustomId = "test_select",
                        Options = new List<StringSelectOption>()
                        {
                            new StringSelectOption()
                            {
                                Label = "Opção 1",
                                Value = "option_1"
                            },

                        }
                    }, context, TestCallback3Async),


                    new ActionRowButtonBuilder()
                        .AddComponent(
                            new ButtonComponent(ButtonStyle.Primary)
                            {
                                CustomId = "test_button",
                                Label = "Testar"
                            },
                            context,
                            TestCallbackAsync

                        ).AddComponent(
                            new ButtonComponent(ButtonStyle.Secondary)
                            {
                                CustomId = "12312312:test_button_2:slash-command",
                                Label = "Testar 2"
                            },
                            context,
                            TestCallback2Async
                        )
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

        private async Task TestCallback2Async(InteractionContext context)
        {
            await context.Response.SendMessageAsync(new Message()
            {
                Content = "Teste 2",
            });
        }


        // Think about how use SlashCommandData in callbacks
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
                ActionRows = [new ActionRowButtonBuilder().AddComponent<InteractionContext>(teste)]
            }, true);
        }
    }
}
