using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Exceptions;
using Disc.NET.Shared.Serializer;
using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Disc.NET.Client.SDK;

public class Client : ClientBase,IClient
{
    private readonly AppConfiguration _appConfiguration;
    public Client(AppConfiguration appConfiguration, HttpClient client) : base(appConfiguration, client)
    {
        _appConfiguration = appConfiguration;
    }

    public async Task SendMessageAsync(string channelId, ApiMessage message, CancellationToken cancellation = default)
    {
        var serializer = DiscNetSerializer.GetInstance();
        var json = serializer.Serialize(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await HttpClient.PostAsync($"channels/{channelId}/messages", content, cancellation).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellation).ConfigureAwait(false);
            throw new DiscNetClientSdkException(error, response.StatusCode);
        }
    }

    public async Task<ApiMessage?> GetMessageAsync(string channelId, string messageId, CancellationToken cancellation = default)
    {
        var serializer = DiscNetSerializer.GetInstance();

        var response = await HttpClient.GetAsync($"channels/{channelId}/messages/{messageId}",cancellation)
            .ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellation).ConfigureAwait(false);
            throw new DiscNetClientSdkException(error, response.StatusCode);
        }
        var content = await response.Content.ReadAsStreamAsync(cancellation).ConfigureAwait(false);
        return await serializer.DeserializeAsync<ApiMessage>(content, cancellation).ConfigureAwait(false);
    }

    public async Task RegisterGlobalSlashCommandAsync(string commandJson, CancellationToken cancellation = default)
    {
        await PostAsync(commandJson, $"applications/{_appConfiguration.ApplicationId}/commands",
            cancellation);
    }

    public async Task RegisterGuildSlashCommandAsync(string commandJson, string guildId, CancellationToken cancellation = default)
    {
        await PostAsync(commandJson, $"applications/{_appConfiguration.ApplicationId}/guilds/{guildId}/commands",
            cancellation);
    }

    public async Task InteractionRespondingAsync(string interactionId, string interactionToken, string responseJson,
        CancellationToken cancellation = default)
    {
        var url = $"https://discord.com/api/v10/interactions/{interactionId}/{interactionToken}/callback";
        await PostAsync(responseJson, url, cancellation);
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