using System.Reflection;

namespace Disc.NET.Handlers;

internal class HandlerFactory<T> where T : IHandler
{
    public static HandlerBase<T> CreateHandlerChain()
    {
        var handlerBaseType = typeof(HandlerBase<T>);

        var handlerTypes = Assembly.GetExecutingAssembly()
            .DefinedTypes
            .Where(t => !t.IsAbstract && handlerBaseType.IsAssignableFrom(t))
            .ToArray();

        if (handlerTypes.Length == 0) throw new ArgumentException();

        var handlerInstances = new HandlerBase<T>[handlerTypes.Length];
        for (int i = 0; i < handlerTypes.Length; i++)
            handlerInstances[i] = (HandlerBase<T>)Activator.CreateInstance(handlerTypes[i])!;

        for (int i = 0; i < handlerInstances.Length - 1; i++)
            handlerInstances[i].SetNext(handlerInstances[i + 1]);

        return handlerInstances[0];
    }

}