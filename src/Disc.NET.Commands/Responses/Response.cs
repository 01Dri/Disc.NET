using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;

namespace Disc.NET.Commands.Responses
{
    public class Response : IResponse
    {
        private readonly IClient _client = ClientSingleton.GetInstance();
        public string ChannelId { get; set; } = string.Empty;
        public string InteractionId { get; set; } = string.Empty;
        public string InteractionToken { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;

        public Response()
        {
            
        }

        public async Task SendMessageAsync(Message message, CancellationToken cancellation = default)
        {
            await _client.SendMessageAsync(ChannelId, message.Build(), cancellation);
        }

        public async Task ReplyAsync(Message message, CancellationToken cancellation = default)
        {
            message.Type = 19;
            message.MessageReference = new ApiMessage()
            {
                MessageId = MessageId
            };
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
