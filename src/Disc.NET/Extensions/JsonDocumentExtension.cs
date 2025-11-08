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
    }
}
