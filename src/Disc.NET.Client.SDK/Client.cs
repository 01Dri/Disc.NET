using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Shared.Exceptions;
using Disc.NET.Shared.Serializer;
using System.Text;

namespace Disc.NET.Client.SDK;

public sealed class Client : ClientBase, IClient
{
    private readonly ClientConfiguration _clientConfiguration;
    private readonly DiscNetSerializer _serializer = DiscNetSerializer.GetInstance();
    public Client(ClientConfiguration clientConfiguration, HttpClient client) : base(clientConfiguration, client)
    {
        _clientConfiguration = clientConfiguration;
    }

    public async Task SendMessageAsync(string channelId, ApiMessage message, CancellationToken cancellation = default)
    {
        await SendMessageAsync(message, channelId, cancellation);
    }

    public async Task<ApiMessage?> GetMessageAsync(string channelId, string messageId, CancellationToken cancellation = default)
    {

        var response = await HttpClient.GetAsync($"channels/{channelId}/messages/{messageId}", cancellation)
            .ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellation).ConfigureAwait(false);
            throw new DiscNetClientSdkException(error, response.StatusCode);
        }
        var content = await response.Content.ReadAsStreamAsync(cancellation).ConfigureAwait(false);
        return await _serializer.DeserializeAsync<ApiMessage>(content, cancellation).ConfigureAwait(false);
    }

    public async Task RegisterGlobalSlashCommandAsync(string commandJson, CancellationToken cancellation = default)
    {
        await PostAsync(commandJson, $"applications/{_clientConfiguration.ApplicationId}/commands",
            cancellation);
    }

    public async Task RegisterGuildSlashCommandAsync(string commandJson, string guildId, CancellationToken cancellation = default)
    {
        await PostAsync(commandJson, $"applications/{_clientConfiguration.ApplicationId}/guilds/{guildId}/commands",
            cancellation);
    }

    public async Task InteractionRespondingAsync(string interactionId, string interactionToken, string responseJson,
        CancellationToken cancellation = default)
    {
        var url = $"https://discord.com/api/v10/interactions/{interactionId}/{interactionToken}/callback";
        await PostAsync(responseJson, url, cancellation);
    }

    public async Task InteractionRespondingAsync(string interactionId, string interactionToken, ApiMessage message,
        bool isEphemeral = false, CancellationToken cancellation = default)
    {
        var teste = _serializer.Serialize(new Teste()
        {
            Type = message.Type!.Value,
            Data = message
        });
        var url = $"https://discord.com/api/v10/interactions/{interactionId}/{interactionToken}/callback";

        await PostAsync(teste, url, cancellation);
    }


    private async Task SendMessageAsync(ApiMessage message, string channelId, CancellationToken cancellation = default)
    {
        var messageJson = _serializer.Serialize(message);
        var content = new StringContent(messageJson, Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"channels/{channelId}/messages", content, cancellation).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellation).ConfigureAwait(false);
            throw new DiscNetClientSdkException(error, response.StatusCode);
        }
    }
    private async Task PostAsync(string json, string uri, CancellationToken cancellation = default)
    {
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await HttpClient.PostAsync(uri, content, CancellationToken.None).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellation).ConfigureAwait(false);
            throw new DiscNetClientSdkException(error, response.StatusCode);
        }
    }   

}