using Disc.NET.Client.SDK;
using Disc.NET.Client.SDK.Interfaces;
using Disc.NET.Client.SDK.Messages;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Shared.Configurations;

namespace Disc.NET.Commands.Contexts
{
    public class CommandContext : IContext
    {
        public string Id { get; set; } = string.Empty;  
        public Author? Author { get; set; }

        public int ChannelType { get; set; } 
        public string ChannelId { get; set; } = string.Empty;
        public string GuildId { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;

        public DateTime? Timestamp { get; set; }
        public int Type { get; set; }
        public DateTime? EditedTimestamp { get; set; }

        public CommandResponse Response { get; set; }
    }

    public class CommandResponse(AppConfiguration appConfiguration)
    {
        private readonly IClient _client = ClientSingleton.GetInstance(appConfiguration);
        public string ChannelId { get; set; } = string.Empty;

        public async Task SendMessageAsync(ApiMessage message, CancellationToken cancellation = default)
        {
            await _client.SendMessageAsync(ChannelId, message, cancellation);
        }
    }
}
