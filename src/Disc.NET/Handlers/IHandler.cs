using Disc.NET.Configurations;
using Disc.NET.Enums;
using System.Text.Json;

namespace Disc.NET.Handlers
{
    internal interface IHandler
    {
        Task HandleAsync(GatewayEvent @event, JsonDocument context, AppOptions options);
        void SetNext(IHandler? next);
    }
}
