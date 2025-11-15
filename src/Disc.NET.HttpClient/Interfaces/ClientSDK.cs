using Disc.NET.Client.SDK.Messages;

namespace Disc.NET.Client.SDK.Interfaces;

public interface IClient
{
    Task SendMessageAsync(string channelId,ApiMessage message, CancellationToken cancellation = default);
    Task<ApiMessage?> GetMessageAsync(string channelId, string messageId, CancellationToken cancellation = default);

}
