using System.Reflection;
using Disc.NET.Commands;
using Disc.NET.Configurations;
using Disc.NET.Enums;

namespace Disc.NET.Handlers;

internal abstract class HandlerBase
{
    private IHandler? _next;

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

    protected IDiscordCommand? GetCommandByAttribute<T>(string commandName)
        where T : Attribute
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
            throw new InvalidOperationException("Could not determine the entry assembly.");

        var nameProperty = typeof(T).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
        if (nameProperty == null)
            throw new InvalidOperationException($"Attribute {typeof(T).Name} must have a public property called 'Name'.");

        var commandType = assembly
            .GetTypes()
            .Where(t => typeof(IDiscordCommand).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .FirstOrDefault(t =>
            {
                var attr = t.GetCustomAttribute<T>();
                if (attr == null) return false;

                var nameValue = nameProperty.GetValue(attr) as string;
                return nameValue != null &&
                       nameValue.Equals(commandName, StringComparison.OrdinalIgnoreCase);
            });

        if (commandType == null)
            return null;

        return (IDiscordCommand)Activator.CreateInstance(commandType)!;
    }


}

