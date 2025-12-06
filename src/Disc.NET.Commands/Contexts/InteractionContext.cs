using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Shared.Configurations;

namespace Disc.NET.Commands.Contexts
{
    public class InteractionContext : IContext
    {
        public Member? Member { get; set; }
        public string Id { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;
        public Channel? Channel { get; set; }
        public int Type { get; set; }
        public int Context { get; set; }

        public InteractionResponse Response { get; set; }
    }

    public class InteractionResponse(AppConfiguration appConfiguration)
    {
        private readonly IClient _client = ClientSingleton.GetInstance(appConfiguration);
        public string ChannelId { get; set; } = string.Empty;

        public async Task SendMessageAsync(ApiMessage message, CancellationToken cancellation = default)
        {
            await _client.SendMessageAsync(ChannelId, message, cancellation);
        }
    }

}
