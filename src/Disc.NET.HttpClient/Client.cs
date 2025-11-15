using System.Net;
using System.Text;
using System.Text.Json;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Exceptions;
using Disc.NET.Shared.Serializer;

namespace Disc.NET.Client.SDK;

public class Client : ClientBase,IClient
{
    public Client(AppConfiguration appConfiguration, HttpClient client) : base(appConfiguration, client)
    {
    }

    public async Task SendMessageAsync(string channelId, ApiMessage message, CancellationToken cancellation = default)
    {
        var serializer = DiscNetSerializer.GetInstance();
        var json = serializer.Serialize(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"channels/{channelId}/messages", content, cancellation);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellation);
            throw new DiscNetClientSdkException(error, response.StatusCode);
        }
    }

    public async Task<ApiMessage?> GetMessageAsync(string channelId, string messageId, CancellationToken cancellation = default)
    {
        var serializer = DiscNetSerializer.GetInstance();

        var response = await HttpClient.GetAsync($"channels/{channelId}/messages/{messageId}",cancellation);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellation);
            throw new DiscNetClientSdkException(error, response.StatusCode);
        }
        var content = await response.Content.ReadAsStreamAsync(cancellation);
        return await serializer.DeserializeAsync<ApiMessage>(content, cancellation);
    }
}