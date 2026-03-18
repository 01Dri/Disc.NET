using Disc.NET.Client.SDK.Messages;
using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Enums;

namespace GenericBot
{
    [SlashCommand("test", InteractionType.SubCommand, "test")]
    public class TestCommand : ISlashCommand
    {
        public async Task RunAsync(InteractionContext context, SlashCommandParamsResult @params)
        {

            var button = new ActionRowButtonComponentBuilder()
              .AddButton(new ButtonComponent(ButtonStyle.Success, "teste")
              {
                  Label = "Teste",
                  Callback = Callback

              });

            var message = new ApiMessage
            {
                MessageComponents = [button]
            };

            await context.Response.SendMessageAsync(message, CancellationToken.None);
        }

        private bool Callback()
        {
            return true;
        }

    }
}
