using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using System.Text.Json;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class SlashCommandHandler : HandlerCommandBase, IHandler
    {
        public SlashCommandHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public override async Task HandleAsync(GatewayEvent @event, JsonDocument contextJson,
            AppConfiguration configuration)
        {
            if (@event != GatewayEvent.InteractionCreate)
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }

            var data = contextJson.RootElement.GetProperty("data");
            var slashCommandResult = Serializer.Deserialize<SlashCommandParamsResult>(data);
            if (slashCommandResult == null)
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }
            var command = (ISlashCommand)
                GetCommandByAttribute<SlashCommandAttribute, InteractionContext>(slashCommandResult.Name);
            if (command == null)
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }

            var context = BuildInteractionContext(contextJson);
            await command.RunAsync(context, slashCommandResult).ConfigureAwait(false);
        }
    }
}
