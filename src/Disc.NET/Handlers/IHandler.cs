using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using System.Text.Json;

namespace Disc.NET.Handlers
{
    internal interface IHandler
    {
        Task HandleAsync(JsonDocument context, AppConfiguration configuration);
        GatewayEvent GetEventType();

    }
}
