using Disc.NET.Client.SDK.Messages.Embeds;

namespace Disc.NET.Client.SDK.Messages;

public class ApiMessage
{
    public string? MessageId { get; set; }
    public string Content { get; set; } = string.Empty;
    public List<Embed> Embeds { get; set; } = [];
    public long Flags {  get; set; }

    public List<object> Components { get; set; } = [];

	public int? Type { get; set; }

    public ApiMessage? MessageReference { get; set; }

    // https://discord.com/developers/docs/resources/message#message-object
}