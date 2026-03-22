using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;

namespace Disc.NET.Commands.Responses
{
    public class InteractionResponse
    {   
        private readonly IClient _client = ClientSingleton.GetInstance();
        public string ChannelId { get; set; } = string.Empty;

        public async Task SendMessageAsync(Message message, CancellationToken cancellation = default)
        {
            await _client.SendMessageAsync(ChannelId, message.Build(), cancellation);
        }
    }
}
