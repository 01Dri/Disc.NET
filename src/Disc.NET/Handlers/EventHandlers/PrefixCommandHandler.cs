using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Configuration;
using Disc.NET.Dispatcher;
using Disc.NET.Enums;
using Disc.NET.Shared.Extensions;
using System.Text.Json;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class PrefixCommandHandler : HandlerCommandBase, IHandler
    {
        public PrefixCommandHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public GatewayEvent GetEventType()
            => GatewayEvent.MessageCreate;

        public async Task HandleAsync(EventHandlerPayload payload, AppConfiguration configuration)
        {
            if (payload.InteractionEventType != InteractionEventType.None) return;
            var content = payload.Data.GetStringProperty("content");
            if (string.IsNullOrEmpty(content)) return;
            var commandModel = BuildCommandModelByEventContent(content);
            var command = (IPrefixCommand)
                GetCommandByAttribute<PrefixCommandAttribute, CommandContext>(commandModel.Name);
            if (command == null) return;
            var context = BuildCommandContext(payload.Data, configuration);
            await command.RunAsync(context).ConfigureAwait(false);
        }
    }
}
