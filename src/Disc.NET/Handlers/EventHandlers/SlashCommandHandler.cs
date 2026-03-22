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
    internal class SlashCommandHandler : HandlerCommandBase, IHandler
    {
        public SlashCommandHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }


        public GatewayEvent GetEventType()
            => GatewayEvent.InteractionCreate;

        public async Task HandleAsync(EventHandlerPayload payload, AppConfiguration configuration)
        {
            if (payload.InteractionEventType != InteractionEventType.ApplicationCommand) return;
            var data = payload.Data.GetJsonStringProperty("data");
            if (string.IsNullOrEmpty(data)) return;
            var slashCommandResult = Serializer.Deserialize<SlashCommandParamsResult>(data);
            if (slashCommandResult == null) return;
            var command = (ISlashCommand)
                GetCommandByAttribute<SlashCommandAttribute, InteractionContext>(slashCommandResult.Name);
            if (command == null) return;
            var context = BuildInteractionContext(payload.Data, configuration);
            await SendInteractionResponseAsync(payload.Data);
            await command.RunAsync(context, slashCommandResult).ConfigureAwait(false);

        }
        private async Task SendInteractionResponseAsync(JsonDocument contextJson)
        {
            var interactionId = contextJson.GetStringProperty("id");
            var interactionToken = contextJson.GetStringProperty("token");
            var responseObject = new
            {
                type = 4,
                data = new
                {
                    content = "Thinking...",
                    flags = 64
                }
            };

            await Client.InteractionRespondingAsync(interactionId, interactionToken,
                Serializer.Serialize(responseObject));
        }
    }

}
