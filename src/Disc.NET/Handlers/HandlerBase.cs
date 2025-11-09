using System.Text.Json;
using Disc.NET.Configurations;
using Disc.NET.Enums;

namespace Disc.NET.Handlers;

internal abstract class HandlerBase
{
    protected IHandler? _next;

    public void SetNext(IHandler? next)
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

