using Disc.NET.Client.SDK.Messages.Components.Enums;
using Disc.NET.Commands;
using Disc.NET.Dispatcher;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using Disc.NET.Shared.Extensions;
using System.Text.Json;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class InteractionCallbackHandler : HandlerCommandBase, IHandler
    {
        public InteractionCallbackHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public GatewayEvent GetEventType()
        {
            return GatewayEvent.InteractionCreate;
        }

        public async Task HandleAsync(EventHandlerPayload payload, AppConfiguration configuration)
        {
            if (payload.MessageType != MessageType.MessageComponent) return;
            var data = payload.Data.GetJsonStringProperty("data");
            if (string.IsNullOrEmpty(data)) return;
            var interaction = Serializer.Deserialize<InteractionCallback>(data);
            if (interaction == null) return;
            var callback = ComponentsCallbacksRepository.Instance.GetCallback(interaction.CustomId);
            if (callback == null) return;
            callback.Invoke();
        }

    }

    internal class InteractionCallback
    {
        public int Id { get; set; }
        public string CustomId { get; set; }
        public MessageComponentType ComponentType { get; set; }
    }
}
