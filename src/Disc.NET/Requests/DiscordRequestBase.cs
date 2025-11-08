namespace Disc.NET.Requests
{
    internal abstract class DiscordRequestBase
    {
        protected readonly HttpClient _httpClient;

        protected DiscordRequestBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
