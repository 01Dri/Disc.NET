using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.MessageBuilders;

namespace GenericBot
{
    [PrefixCommand("helloword")]
    public class PrefixPrefixCommandTest : IPrefixCommand
    {

        public async Task RunAsync(CommandContext context, List<string> @params)
        {
            await context.Response.ReplyAsync(new Message()
            {
                Content = "Olá Mundo",
                Embeds =
                [
                    new()
                    {
                        Title = "Título do Embed",
                        Description = "Descrição do Embed"
                    }
                ],
                ActionRows = new List<IActionRowBuilder>
                { 
                    new ActionRowButtonBuilder().AddComponent
                    (
                        new ButtonComponent(ButtonStyle.Primary)
                        {
                            Label = "Clique aqui",
                            CustomId = "button_click"
                        },
                        context, Test)
                }
            });
        }

        private async Task Test(CommandContext context)
        {
            await context.Response.SendMessageAsync(new Message()
            {
                Content = "Olá Mundo",
            });
        }
    }
}
