using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using Disc.NET.Shared.Extensions;
using System.Text.Json;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class PrefixCommandHandler : HandlerCommandBase, IHandler
    {
        public PrefixCommandHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public override async Task HandleAsync(GatewayEvent @event, JsonDocument contextJson, AppConfiguration configuration)
        {
            if (@event != GatewayEvent.MessageCreate)
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }

            var content = contextJson.GetStringProperty("content");
            if (string.IsNullOrEmpty(content))
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }
            var commandModel = BuildCommandModelByEventContent(content);
            if (commandModel.Prefix != configuration.BotPrefix)
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }

            var command = (IPrefixCommand)
                GetCommandByAttribute<PrefixCommandAttribute, CommandContext>(commandModel.Name);

            if (command == null)
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }

            var context = BuildCommandContext(contextJson, configuration);
            await command.RunAsync(context, commandModel.Params).ConfigureAwait(false);
        }

    }
}
