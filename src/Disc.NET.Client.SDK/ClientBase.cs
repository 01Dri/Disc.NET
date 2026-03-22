namespace Disc.NET.Client.SDK
{
    public abstract class ClientBase
    {

        protected readonly HttpClient HttpClient;
        protected ClientConfiguration ClientConfiguration { get; }
         
        protected ClientBase(ClientConfiguration clientConfiguration, HttpClient client)
        {
            ClientConfiguration = clientConfiguration;
            HttpClient = client;
            HttpClient.BaseAddress = new Uri("https://discord.com/api/v10/");
            HttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", ClientConfiguration.Token);
        }
    }
}
