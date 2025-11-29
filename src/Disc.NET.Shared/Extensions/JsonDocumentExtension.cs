using System.Reflection;
using System.Text.Json;

namespace Disc.NET.Shared.Extensions
{
    internal static class JsonDocumentExtension
    {

        public static int? GetIntDeepProperty(this JsonDocument document, string propertyInit, string propertyResult)
        {
            if (document.RootElement.TryGetProperty(propertyInit, out var d) &&
                d.TryGetProperty(propertyResult, out var interval) &&
                interval.ValueKind == JsonValueKind.Number)
            {
                return interval.GetInt32();
            }

            return null;
        }

        public static int? GetIntProperty(this JsonDocument document, string property)
        {
            if (document.RootElement.TryGetProperty(property, out var sElement) &&
                sElement.ValueKind == JsonValueKind.Number)
            {
                return sElement.GetInt32();
            }

            return null;
        }

        public static string? GetStringDeepProperty(this JsonDocument document, string propertyInit,
            string propertyResult)
        {
            if (document.RootElement.TryGetProperty(propertyInit, out var d) &&
                d.TryGetProperty(propertyResult, out var interval) &&
                interval.ValueKind == JsonValueKind.String)
            {
                return interval.GetString();
            }

            return null;
        }

        public static string? GetStringProperty(this JsonDocument document, string property)
        {
            if (document.RootElement.TryGetProperty(property, out var tElement))
                return tElement.GetString();

            return null;
        }


        public static DateTime? GetDateTimeProperty(this JsonDocument document, string property)
        {
            if (document.RootElement.TryGetProperty(property, out var idElement))
            {
                if (idElement.ValueKind == JsonValueKind.Null)
                {
                    return null;
                }

                return idElement.GetDateTime();
            }

            return null;
        }

        public static JsonDocument? GetEventContextData(this JsonDocument document)
        {
            if (document.RootElement.TryGetProperty("d", out var dElement) &&
                dElement.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
            {
                try
                {
                    return JsonSerializer.Deserialize<JsonDocument>(dElement.GetRawText());
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public static string? GetJsonStringProperty(this JsonDocument document, string propertyName)
        {
            if (document.RootElement.TryGetProperty(propertyName, out var propertyElement))
                return JsonSerializer.Serialize(propertyElement);

            return null;
        }


        public static string? GetDeepJsonStringProperty(this JsonDocument document, string propertyInit, string propertyResult)
        {
            if (document.RootElement.TryGetProperty(propertyInit, out var d) &&
                d.TryGetProperty(propertyResult, out var interval))
            {
                return JsonSerializer.Serialize(interval);
            }

            return null;
        }

        public static JsonDocument? GetJsonDocumentProperty(this JsonDocument document, string propertyName)
        {
            if (document.RootElement.TryGetProperty(propertyName, out var propertyElement))
                return propertyElement.Deserialize<JsonDocument>();

            return null;
        }

        public static string? GetJsonStringProperty(this JsonDocument document)
        {
            return JsonSerializer.Serialize(document);
        }

    }

}
