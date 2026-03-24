using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Shared.Serializer;

namespace Disc.NET.Commands.Responses
{
    public class InteractionResponse
    {   
        private readonly IClient _client = ClientSingleton.GetInstance();
        private readonly DiscNetSerializer _serializer = DiscNetSerializer.GetInstance();
        public string ChannelId { get; set; } = string.Empty;
        public string InteractionId { get; set; } = string.Empty;
        public string InteractionToken { get; set; } = string.Empty;

        public async Task SendMessageAsync(Message message, CancellationToken cancellation = default)
        {
            await _client.SendMessageAsync(ChannelId, message.Build(), cancellation);
        }

        public async Task SendInteractionResponseAsync(Message message, bool ephemeral = false, CancellationToken cancellation = default)
        {
            await _client.SendInteractionResponseAsync
            (
                InteractionId,
                InteractionToken, 
                message.Build(),
                ephemeral,
                cancellation
            );
        }
    }
}
