using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.Enums;
using Disc.NET.Commands.MessageBuilders;

namespace GenericBot
{

    [SlashCommand("testo", InteractionType.SubCommand, "Ver as informações de tempo da sua cidade")]
    public class TestCommand : ISlashCommand
    {
        public async Task RunAsync(InteractionContext context, CancellationToken cancellation = default)
        {
            var message = new MessageBuilder()
                .WithContent("teste")
                .WithEmbed(embed => embed
                    .SetTitle("Embed de teste")
                    .SetDescription("Mensagem gerada com MessageBuilder + EmbedBuilder")
                    .SetColor(0x5865F2)
                    .AddField("Comando", "/test", inline: true))
                .WithActionRow(new ActionRowBuilder()
                    .AddButton("Testar", "test_button", ButtonStyle.Primary, context, TestCallbackAsync)
                    .AddLinkButton("Testar 2", "https://www.linkedin.com/in/dridev/")
                ).Build();

            await context.Response.SendMessageAsync(message, cancellation);
        }

        private async Task TestCallbackAsync(InteractionContext context)
        {
            await context.Response.SendMessageAsync(new MessageBuilder()
                .WithContent("Teste")
                .WithEmbed(embed => embed
                    .SetTitle("Callback executado")
                    .SetDescription("Resposta criada via MessageBuilder")
                    .SetColor(0x57F287))
                .Build());
        }

        private async Task TestCallback3Async(InteractionContext context)
        {
            var teste = new ButtonComponent(ButtonStyle.Danger)
            {
                Label = "Teste 3",
                CustomId = "test_button_3",
            };
            await context.Response.SendInteractionResponseAsync(new Message()
            {
                Content = "teste",
                ActionRows = [new ActionRowBuilder().AddButton<InteractionContext>(teste)]
            }, true);
        }
    }
}
