using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Commands;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json;

namespace Disc.NET.Handlers;

internal abstract class HandlerBase<TContext> where TContext : IContext
{
    private IHandler? _next;
    protected IClient Client;

    protected HandlerBase(AppConfiguration appConfiguration)
    {
        Client = CommandBase.GetInstance(appConfiguration).UseClient();
    }


    public void SetNext(IHandler? next)
    {
        _next = next;
    }
    public virtual async Task HandleAsync(GatewayEvent @event, JsonDocument contextJson, AppConfiguration configuration)
    {
        if (_next != null)
        {
            await _next.HandleAsync(@event, contextJson, configuration);
        }
    }

    protected abstract TContext BuildContext(JsonDocument contextJson);
}

