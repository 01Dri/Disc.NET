using System.Text.Json;

namespace Disc.NET.Extensions
{
    internal static class JsonDocumentExtension
    {
        public static int GetHeartbeatInterval(this JsonDocument document)
        {
            return document.RootElement.GetProperty("d").GetProperty("heartbeat_interval").GetInt32();
        }

        public static int GetLastSequenceEventNumber(this JsonDocument document)
        {
            var sequenceProperty = document.RootElement.GetProperty("s");
            return sequenceProperty.ValueKind == JsonValueKind.Null ? 0 : sequenceProperty.GetInt32();
        }

        public static string? GetEventName(this JsonDocument document)
        {
            return document.RootElement.GetProperty("t").GetString();
        }

        public static int GetOpCode(this JsonDocument document)
        {
            return document.RootElement.GetProperty("op").GetInt32();
        }

        public static JsonDocument GetEventContextData(this JsonDocument document)
        {
            var property = document.RootElement.GetProperty("d");
            return JsonDocument.Parse(JsonSerializer.Serialize(property));
        }

        public static string GetAuthor(this JsonDocument document)
        {
            return JsonSerializer.Serialize(document.RootElement.GetProperty("author"));
        }

        public static string GetContent(this JsonDocument document)
        {
            return document.RootElement.GetProperty("content").GetString() ?? string.Empty;
        }

        public static string GetChannelId(this JsonDocument document)
        {
            return document.RootElement.GetProperty("channel_id").GetString() ?? string.Empty;
        }

    }
}
