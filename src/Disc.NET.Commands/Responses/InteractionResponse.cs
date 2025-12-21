using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Shared.Configurations;

namespace Disc.NET.Commands.Responses
{
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
