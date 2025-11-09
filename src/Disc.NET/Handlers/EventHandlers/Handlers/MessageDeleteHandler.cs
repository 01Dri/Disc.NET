using Disc.NET.Configurations;
using Disc.NET.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disc.NET.Handlers.EventHandlers.Handlers
{
    internal class MessageDeleteHandler : HandlerBase, IHandler
    {
        public override async Task HandleAsync(DiscordWebSocketEventType eventType, string contextJson,
            AppOptions options)
        {
            if (eventType != DiscordWebSocketEventType.MessageDelete)
            {
                await base.HandleAsync(eventType, contextJson, options);
                return;
            }
            Console.WriteLine("MESSAGE DELETED: " + contextJson);
        }
    }
}
