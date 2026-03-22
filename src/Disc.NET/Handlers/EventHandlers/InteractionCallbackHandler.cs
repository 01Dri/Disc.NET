using Disc.NET.Commands;
using Disc.NET.Configuration;
using Disc.NET.Dispatcher;
using Disc.NET.Enums;
using Disc.NET.Shared.Constraints;
using Disc.NET.Shared.Extensions;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class InteractionCallbackHandler : HandlerCommandBase, IHandler
    {
        public InteractionCallbackHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public GatewayEvent GetEventType()
            => GatewayEvent.InteractionCreate;

        public async Task HandleAsync(EventHandlerPayload payload, AppConfiguration configuration)
        {
            if (payload.InteractionEventType != InteractionEventType.MessageComponent) return;
            var customIdWithSuffix = payload.Data.GetStringDeepProperty("data", "custom_id");
            if (string.IsNullOrEmpty(customIdWithSuffix))
            {
                return;
            }
            var customId = CallbackCustomIdHelper.GetCustomId(customIdWithSuffix);
            if (string.IsNullOrEmpty(customIdWithSuffix))
            {
                return;
            }
            var guildId = payload.Data.GetStringProperty("guild_id");
            if (guildId != null)
            {
                var callbackCustomId = CallbackCustomIdHelper.GetCallbackCustomId(guildId, customIdWithSuffix);
                var context = BuildContextByCustomIdCallbackType(payload, callbackCustomId.CallbackType,  configuration);
                await ComponentCallbackRepository.InvokeCallbackAsync(callbackCustomId.Id, context);
            }
        }
    }
}
