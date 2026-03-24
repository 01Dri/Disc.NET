// See https://aka.ms/new-console-template for more information


using Disc.NET;
using Disc.NET.Configuration;
using Disc.NET.Enums;


var builder = new AppBuilder();
builder.Services.AddHttpClient();
builder.AddConfiguration(new AppConfiguration(Environment.GetEnvironmentVariable("GENERIC_BOT_TOKEN")!)
{
    Intents = [GatewayIntent.MESSAGE_CONTENT, GatewayIntent.GUILD_MESSAGES],
    ApplicationId = long.Parse(Environment.GetEnvironmentVariable("GENERIC_BOT_APPLICATION_ID")!),
    BotPrefix = '?'
});

var app = builder.Build();
await app.RunAsync();
