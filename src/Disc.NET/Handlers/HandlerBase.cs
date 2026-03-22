using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Commands.Contexts;
using Disc.NET.Configuration;
using Disc.NET.Shared.Serializer;
using System.Text.Json;

namespace Disc.NET.Handlers;

internal abstract class HandlerBase<TContext> where TContext : IContext
{
    protected IClient Client;
    protected DiscNetSerializer Serializer;
    protected HandlerBase(AppConfiguration appConfiguration)
    {
        ClientSingleton.Configure(appConfiguration.Token, appConfiguration.ApplicationId);
        Client = ClientSingleton.GetInstance();
        Serializer = DiscNetSerializer.GetInstance();
    }
    protected abstract CommandContext BuildCommandContext(JsonDocument contextJson, AppConfiguration appConfiguration);
    protected abstract InteractionContext BuildInteractionContext(JsonDocument contextJson, AppConfiguration appConfiguration);


}

