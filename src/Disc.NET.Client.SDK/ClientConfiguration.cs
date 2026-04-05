namespace Disc.NET.Client.SDK
{
    public class ClientConfiguration
    {
        public string Token { get; set; }
        public long ApplicationId { get; set; }
        public ClientConfiguration(string token, long applicationId)
        {
            Token = token;
            ApplicationId = applicationId;
        }
    }
}
