using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.Responses
{
    public class CommandResponse
    {
        private readonly IClient _client = ClientSingleton.GetInstance();
        public string ChannelId { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;


        public async Task SendMessageAsync<T>(Message<T> message, CancellationToken cancellation = default) where T : class, IContext
        {
            await _client.SendMessageAsync(ChannelId, message.Build(), cancellation);
        }

        public async Task ReplyAsync<T>(Message<T> message, CancellationToken cancellation = default) where T : class, IContext
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
