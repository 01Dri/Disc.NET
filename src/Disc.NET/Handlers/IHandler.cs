using Disc.NET.Configuration;
using Disc.NET.Dispatcher;
using Disc.NET.Enums;
using System.Text.Json;

namespace Disc.NET.Handlers
{
    internal interface IHandler
    {
        Task HandleAsync(EventHandlerPayload payload, AppConfiguration configuration);
        GatewayEvent GetEventType();

    }
}
