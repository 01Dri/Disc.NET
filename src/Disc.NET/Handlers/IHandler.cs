using Disc.NET.Dispatcher;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;

namespace Disc.NET.Handlers
{
    internal interface IHandler
    {
        Task HandleAsync(EventHandlerPayload payload, AppConfiguration configuration);
        GatewayEvent GetEventType();

    }
}
