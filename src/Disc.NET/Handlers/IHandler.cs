using Disc.NET.Configurations;
using Disc.NET.Enums;
using Disc.NET.Models.Commands;
using System.Text.Json;
using Disc.NET.Models;

namespace Disc.NET.Handlers
{
    internal interface IHandler
    {
        Task HandleAsync(GatewayEvent @event, JsonDocument context, AppOptions options);
        void SetNext(IHandler? next);
    }
}
