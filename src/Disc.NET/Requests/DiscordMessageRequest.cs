using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Disc.NET.Requests
{
    internal class DiscordMessageRequest : DiscordRequestBase
    {
        public DiscordMessageRequest(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<bool> SendTextAsync(string text, string token)
        {
            var payload = new
            {
                content = text
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", token);
            var body = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "https://discord.com/api/v10/channels/1334886765399511042/messages", // ID do canal deve vir de algum lugar
                body
            );
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
