using Disc.NET.Client.SDK.Messages;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;

namespace GenericBot
{
    [PrefixCommand("helloword")]
    public class PrefixPrefixCommandTest : IPrefixCommand
    {

        public async Task RunAsync(CommandContext context, List<string> @params)
        {
            await context.Response.ReplyAsync(new Message<CommandContext>()
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
        }

    }
}
