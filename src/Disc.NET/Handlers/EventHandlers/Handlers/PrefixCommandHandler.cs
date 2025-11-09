using System.Reflection;
using Disc.NET.Attributes.Commands;
using Disc.NET.Commands;
using Disc.NET.Configurations;
using Disc.NET.Enums;

namespace Disc.NET.Handlers.EventHandlers.Handlers
{
    internal class PrefixCommandHandler : HandlerBase, IHandler
    {

        public PrefixCommandHandler()
        {
        }

        public override async Task HandleAsync(DiscordWebSocketEventType eventType, string contextJson, AppOptions options)
        {
            if (eventType != DiscordWebSocketEventType.MessageCreate)
            {
                await base.HandleAsync(eventType, contextJson, options);
                return;
            }

            if (contextJson[0] != options.BotPrefix)
            {
                await base.HandleAsync(eventType, contextJson, options);
                return;
            }
            var commandName = contextJson[1..].Split(' ')[0].Trim().ToLowerInvariant();

            IDiscordCommand? command = GetCommandByAttribute<PrefixCommandAttribute>(commandName);

            if (command == null)
            {
                await base.HandleAsync(eventType, contextJson, options);
                return;
            }

            await command.RunAsync();
        }

    }
}
