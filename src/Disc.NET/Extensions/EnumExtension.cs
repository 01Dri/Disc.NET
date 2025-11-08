using System.ComponentModel;
using System.Reflection;
using Disc.NET.Enums;

namespace Disc.NET.Extensions
{
    internal static class EnumExtension
    {
        public static string GetEnumDescription(this Enum value)
        {
            var member = value.GetType()
                .GetMember(value.ToString(), BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault();

            if (member?.GetCustomAttribute<DescriptionAttribute>() is { } descriptionAttr)
                return descriptionAttr.Description;

            return value.ToString();
        }

        public static DiscordWebSocketEventType ToDiscordWebSocketEventsType(this string eventStr)
        {
            if (string.IsNullOrWhiteSpace(eventStr))
                return DiscordWebSocketEventType.None;

            var normalized = eventStr.Trim();

            if (Enum.TryParse(typeof(DiscordWebSocketEventType), normalized, ignoreCase: true, out var directMatch))
                return (DiscordWebSocketEventType)directMatch;

            foreach (var field in typeof(DiscordWebSocketEventType).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = field.GetCustomAttribute<DescriptionAttribute>();
                if (attr != null && string.Equals(attr.Description, normalized, StringComparison.OrdinalIgnoreCase))
                    return (DiscordWebSocketEventType)field.GetValue(null)!;
            }

            return DiscordWebSocketEventType.None;
        }

        public static int  GetIntIntents(this IEnumerable<DiscordGatewayIntentsType> intents)
        {
            DiscordGatewayIntentsType combined = 0;
            foreach (var intent in intents)
                combined |= intent; 

            return (int)combined;
        }

        public static int GetIntIntent(DiscordGatewayIntentsType intentsType)
        {
            return (int)intentsType;
        }

    }
}
