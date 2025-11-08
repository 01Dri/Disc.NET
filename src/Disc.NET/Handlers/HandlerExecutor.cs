using Disc.NET.Configurations;
using Disc.NET.Enums;
using Disc.NET.Handlers.EventHandlers.MessageCreate;

namespace Disc.NET.Handlers
{
    internal class HandlerExecutor
    {

        public static async Task ExecuteHandlerAsync(DiscordWebSocketEventType eventType, string contextJson, AppOptions options)
        {
            switch (eventType)
            {
                case DiscordWebSocketEventType.MessageCreate:
                    Console.WriteLine("MESSAGE_CREATE event received!");
                    var messageCreateHandler = HandlerFactory<IMessageCreateHandler>.CreateHandlerChain();
                    await messageCreateHandler.HandleAsync(eventType, contextJson, options);
                    break;
                default:
                    return;
            }
        }
    }
}
