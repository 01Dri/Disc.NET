using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;

namespace Disc.NET.Commands.Responses
{
    public class CommandResponse
    {
        private readonly IClient _client = ClientSingleton.GetInstance();
        public string ChannelId { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;


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
    }
}
