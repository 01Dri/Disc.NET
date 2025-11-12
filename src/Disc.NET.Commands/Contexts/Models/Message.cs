using System.Text.Json.Serialization;

namespace Disc.NET.Commands.Contexts.Models
{
    public class Message
    {

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

    }
}
