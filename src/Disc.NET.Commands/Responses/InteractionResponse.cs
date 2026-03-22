using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.Responses
{
    public class InteractionResponse()
    {   
        private readonly IClient _client = ClientSingleton.GetInstance();
        public string ChannelId { get; set; } = string.Empty;

        public async Task SendMessageAsync<T>(Message<T> message, CancellationToken cancellation = default) where T : class, IContext
        {
            await _client.SendMessageAsync(ChannelId, message.Build(), cancellation);
        }
    }
}
