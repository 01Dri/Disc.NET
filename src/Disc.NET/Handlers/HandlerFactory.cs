using System.Reflection;

namespace Disc.NET.Handlers;

internal class HandlerFactory 
{
    public static IHandler CreateHandlerChain()
    {
        var handlerTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        return CreateChainRecursive(handlerTypes, 0)!;
    }

    private static IHandler? CreateChainRecursive(List<Type> handlerTypes, int index)
    {
        if (index >= handlerTypes.Count)
            return null;

        var handlerInstance = (IHandler)Activator.CreateInstance(handlerTypes[index])!;
        var nextHandler = CreateChainRecursive(handlerTypes, index + 1);

        handlerInstance.SetNext(nextHandler);
        return handlerInstance;
    }

}