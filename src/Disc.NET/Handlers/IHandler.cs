using System.Text.Json;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;

namespace Disc.NET.Handlers
{
    internal interface IHandler
    {
        Task HandleAsync(GatewayEvent @event, JsonDocument context, AppConfiguration configuration);
        void SetNext(IHandler? next);
    }
}
