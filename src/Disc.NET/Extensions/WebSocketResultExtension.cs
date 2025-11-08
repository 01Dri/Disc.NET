using System.Text;
using System.Text.Json;

namespace Disc.NET.Extensions
{
    internal static class WebSocketResultExtension
    {
        public static string GetString(this System.Net.WebSockets.WebSocketReceiveResult result, byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        public static JsonDocument GetJsonDocument(this System.Net.WebSockets.WebSocketReceiveResult result, byte[] buffer)
        {
            var json = result.GetString(buffer);
            return JsonDocument.Parse(json);
        }
    }
}
