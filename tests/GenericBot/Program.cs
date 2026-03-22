// See https://aka.ms/new-console-template for more information


using Disc.NET;
using Disc.NET.Configuration;
using Disc.NET.Enums;



App app = new App().WithDebugLogger();
var token = Environment.GetEnvironmentVariable("GENERIC_BOT_TOKEN")!;
var applicationId = Environment.GetEnvironmentVariable("GENERIC_BOT_APPLICATION_ID")!;
var appConfiguration =
    new AppConfiguration(token)
    {
        Intents = [GatewayIntent.MESSAGE_CONTENT, GatewayIntent.GUILD_MESSAGES],
        ApplicationId = long.Parse(applicationId),
        BotPrefix = '?'
    };

// Tratar os lifetimes, por padrão é InstancePerDependency (uma nova toda vez que é resolvida)
app.UseDependencyInjection(appConfiguration).WithHttpClient();
await app.RunAsync(appConfiguration);
