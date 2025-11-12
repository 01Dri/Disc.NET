using Disc.NET.Commands;
using Disc.NET.Configurations;
using Disc.NET.Enums;
using System.Reflection;
using System.Text.Json;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Handlers;

internal abstract class HandlerBase<TContext> where TContext : IContext
{
    private IHandler? _next;

    public void SetNext(IHandler? next)
    {
        _next = next;
    }
    public virtual async Task HandleAsync(GatewayEvent @event, JsonDocument contextJson, AppOptions options)
    {
        if (_next != null)
        {
            await _next.HandleAsync(@event, contextJson, options);
        }
    }


    protected ICommand<TK>? GetCommandByAttribute<T, TK>(string commandName)
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

        return commandType != null ? (ICommand<TK>)Activator.CreateInstance(commandType)! : null;
    }

    protected abstract TContext BuildContext(JsonDocument contextJson);
}

