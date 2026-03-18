using System.Text.Json.Serialization;

namespace Disc.NET.Client.SDK.Messages.Components
{
	public interface IMessageComponent
	{
		[JsonIgnore]
		Func<bool> Callback { get; set; }
		string? CustomId { get; set; }

    }
}	
