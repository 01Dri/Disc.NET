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

        public GatewayEvent GetEventType()
            => GatewayEvent.MessageCreate;

        public async Task HandleAsync(JsonDocument contextJson, AppConfiguration configuration)
        {
            var content = contextJson.GetStringProperty("content");
            if (string.IsNullOrEmpty(content)) return;
            var commandModel = BuildCommandModelByEventContent(content);
            var command = (IPrefixCommand)
                GetCommandByAttribute<PrefixCommandAttribute, CommandContext>(commandModel.Name);
            if (command == null) return;
            var context = BuildCommandContext(contextJson, configuration);
            await command.RunAsync(context, commandModel.Params).ConfigureAwait(false);
        }
    }
}
