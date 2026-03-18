using System.ComponentModel;
using System.Reflection;
using Disc.NET.Shared.Enums;

namespace Disc.NET.Shared.Extensions
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

        //public static GatewayEvent ToGatewayEventType(this string eventStr)
        //{
        //    if (string.IsNullOrWhiteSpace(eventStr))
        //        return GatewayEvent.None;

        //    var normalized = eventStr.Trim();

        //    if (Enum.TryParse(typeof(GatewayEvent), normalized, ignoreCase: true, out var directMatch))
        //        return (GatewayEvent)directMatch;

        //    foreach (var field in typeof(GatewayEvent).GetFields(BindingFlags.Public | BindingFlags.Static))
        //    {
        //        var attr = field.GetCustomAttribute<DescriptionAttribute>();
        //        if (attr != null && string.Equals(attr.Description, normalized, StringComparison.OrdinalIgnoreCase))
        //            return (GatewayEvent)field.GetValue(null)!;
        //    }

        //    return GatewayEvent.None;
        //}

        public static T? ToEnum<T>(this string eventStr) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(eventStr))
                return null;

            var normalized = eventStr.Trim();

            if (Enum.TryParse<T>(normalized, true, out var directMatch))
                return directMatch;

            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = field.GetCustomAttribute<DescriptionAttribute>();

                if (attr != null &&
                    string.Equals(attr.Description, normalized, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)field.GetValue(null)!;
                }
            }

            return null;
        }


        public static T? ToEnum<T>(this int? value) where T : struct, Enum
        {
            if (value == null) return null;
            if (Enum.IsDefined(typeof(T), value))
                return (T)Enum.ToObject(typeof(T), value);

            return null;
        }

        public static int  GetIntIntents(this IEnumerable<GatewayIntent> intents)
        {
            GatewayIntent combined = 0;
            foreach (var intent in intents)
                combined |= intent; 

            return (int)combined;
        }

        public static int GetIntIntent(GatewayIntent intent)
        {
            return (int)intent;
        }

    }
}
