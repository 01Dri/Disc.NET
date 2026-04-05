using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Constraints;

namespace Disc.NET.Commands.MessageBuilders
{
    public abstract class ActionRowBuilderBase
    {
        protected void RegisterComponentCallback<T>(IMessageComponent component, T context, Func<T, Task>? callback) where T : ContextBase
        {
            if (callback is null)
                return;

            if (string.IsNullOrWhiteSpace(component.CustomId))
                throw new ArgumentException("CustomId must be set.");


            var guildId = string.Empty;
            var callbackType = CallbackType.None;
            // maybe context can be single for both command and interaction, but for now we will just check both types
            if (context is CommandContext baseCtx)
            {
                guildId = baseCtx.GuildId;
            }

            else if (context is InteractionContext baseCtxI)
            {
                guildId = baseCtxI.GuildId;
            }

            if (typeof(CommandContext).IsAssignableFrom(typeof(T)))
            {
                callbackType = CallbackType.PrefixCommand;
            }
            else
            {
                callbackType = CallbackType.InteractionCommand;
            }   
            var customIdCallback = CallbackCustomIdHelper.BuildCustomId(guildId, component.CustomId, callbackType);
            component.CustomId = CallbackCustomIdHelper.BuildCustomId(component.CustomId, callbackType);

            ComponentCallbackRepository.RegisterCallback(
                customIdCallback,
                async (ctx) =>
                {
                    if (ctx is T typedCtx)
                    {
                        await callback(typedCtx);
                        return;
                    }

                    throw new InvalidCastException(
                        $"Invalid context type. Expected {typeof(T).Name}, got {ctx.GetType().Name}");
                });
        }
    }
}
