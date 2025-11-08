using Disc.NET.Configurations;
using Disc.NET.Enums;
using Disc.NET.Requests;

namespace Disc.NET.Handlers.EventHandlers.MessageCreate.Handlers
{
    internal class PrefixCommandHandler : HandlerBase<IMessageCreateHandler>
    {
        private readonly DiscordMessageRequest _discordMessageRequest;

        public PrefixCommandHandler()
        {
            _discordMessageRequest = new DiscordMessageRequest(new HttpClient());
        }

        public override async Task HandleAsync(DiscordWebSocketEventType eventType, string contextJson, AppOptions options)
        {
            if (string.IsNullOrEmpty(contextJson) || contextJson.Length < 2)
            {
                await base.HandleAsync(eventType, contextJson, options);
                return;
            }
            if (contextJson[0] != options.BotPrefix)
            {
                await base.HandleAsync(eventType, contextJson, options);
                return;
            }
            var command = contextJson[1..].Trim();
            if (string.IsNullOrWhiteSpace(command))
            {
                await base.HandleAsync(eventType, contextJson , options);
                return;
            }
            Console.WriteLine("PREFIX COMMAND: " + command);
            // Chamar as classes que implementam comando
            await _discordMessageRequest.SendTextAsync("Olá mundo!",
                "MTQzNjE3ODQ0ODkwMDE2MTYyMA.Gevo76.nqkBj12AGeZI-3BmCSLa6oz1_ETbvpb9tiXCFA");
        }

    }
}
