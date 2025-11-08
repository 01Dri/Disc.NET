using Disc.NET.Configurations;
using Disc.NET.Enums;

namespace Disc.NET.Handlers;

internal abstract class HandlerBase<T> where T : IHandler
{
    private HandlerBase<T>? _next;


    public void SetNext(HandlerBase<T>? next)
    {
        _next = next;
    }
    public virtual async Task HandleAsync(DiscordWebSocketEventType eventType, string contextJson, AppOptions options)
    {
        if (_next != null)
        {
            await _next.HandleAsync(eventType, contextJson, options);
        }
    }
}

