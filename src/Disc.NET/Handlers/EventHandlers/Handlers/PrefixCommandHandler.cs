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
            Console.WriteLine("PREFIX COMMAND: ");
        }

    }
}
