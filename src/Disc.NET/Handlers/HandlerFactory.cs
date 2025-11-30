using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Disc.NET.Shared.Configurations;

namespace Disc.NET.Handlers;

internal sealed class HandlerFactory
{
    public static IHandler CreateHandlerChain(AppConfiguration appConfiguration)
    {
        var handlerTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        return CreateChainRecursive(handlerTypes, 0, appConfiguration)!;
    }
        
    private static IHandler? CreateChainRecursive(List<Type> handlerTypes, int index, AppConfiguration appConfiguration)
    {
        if (index >= handlerTypes.Count)
            return null;


        var handlerInstance = (IHandler)Activator.CreateInstance(handlerTypes[index], appConfiguration)!;
        var nextHandler = CreateChainRecursive(handlerTypes, index + 1, appConfiguration);

        handlerInstance.SetNext(nextHandler);
        return handlerInstance;
    }

}