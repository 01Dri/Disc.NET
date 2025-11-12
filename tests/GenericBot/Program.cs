// See https://aka.ms/new-console-template for more information


using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Disc.NET;
using Disc.NET.Configurations;
using Disc.NET.Enums;


App app = new App().WithDebugLogger();
var options = new AppOptions()
{
    Intents = [GatewayIntent.MESSAGE_CONTENT, GatewayIntent.GUILD_MESSAGES]
};
await app.RunAsync("MTQzNjE3ODQ0ODkwMDE2MTYyMA.Gevo76.nqkBj12AGeZI-3BmCSLa6oz1_ETbvpb9tiXCFA", options);