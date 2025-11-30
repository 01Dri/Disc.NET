// See https://aka.ms/new-console-template for more information


using Disc.NET;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;



App app = new App().WithDebugLogger();
var appConfiguration = new AppConfiguration("MTQzNjE3ODQ0ODkwMDE2MTYyMA.Gevo76.nqkBj12AGeZI-3BmCSLa6oz1_ETbvpb9tiXCFA")
{
    Intents = [GatewayIntent.MESSAGE_CONTENT, GatewayIntent.GUILD_MESSAGES],
    ApplicationId = 1436178448900161620,
    BotPrefix = '?'
};

// Tratar os lifetimes, por padrão é InstancePerDependency (uma nova toda vez que é resolvida)
app.UseDependencyInjection(appConfiguration).WithHttpClient();
await app.RunAsync(appConfiguration);