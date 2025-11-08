using Disc.NET.Configurations;
using Disc.NET.Enums;

namespace Disc.NET.Handlers
{
    internal interface IHandler
    {
        Task HandleAsync(DiscordWebSocketEventType eventType, string context, AppOptions options);
        void SetNext(IHandler? next);
    }
}
