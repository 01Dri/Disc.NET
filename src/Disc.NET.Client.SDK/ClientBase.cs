using Disc.NET.Shared.Configurations;

namespace Disc.NET.Client.SDK
{
    public abstract class ClientBase
    {

        protected readonly HttpClient HttpClient;
        protected AppConfiguration AppConfiguration { get; }
         

        protected ClientBase(AppConfiguration appConfiguration, HttpClient client)
        {
            AppConfiguration = appConfiguration;
            HttpClient = client;
            HttpClient.BaseAddress = new Uri("https://discord.com/api/v10/");
            HttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", AppConfiguration.Token);
        }
    }
}
