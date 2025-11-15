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


    protected ICommand<TK>? GetCommandByAttribute<T, TK>(string commandName, AppConfiguration configuration)
        where T : Attribute where TK : IContext
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
            throw new InvalidOperationException("Could not determine the entry assembly.");

        var nameProperty = typeof(T).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
        if (nameProperty == null)
            throw new InvalidOperationException($"Attribute {typeof(T).Name} must have a public property called 'Name'.");

        var commandType = assembly
            .GetTypes()
            .Where(t => typeof(ICommand<TK>).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .FirstOrDefault(t =>
            {
                var attr = t.GetCustomAttribute<T>();
                if (attr == null) return false;

                var nameValue = nameProperty.GetValue(attr) as string;
                return nameValue != null &&
                       nameValue.Equals(commandName, StringComparison.OrdinalIgnoreCase);
            });
        // Temporally code I need other way to get singleton instance of CommandBase
        CommandBase.GetInstance(configuration);
        return commandType != null ? (ICommand<TK>)Activator.CreateInstance(commandType)! : null;
    }

    protected abstract TContext BuildContext(JsonDocument contextJson);
}

