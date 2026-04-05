using System.ComponentModel;
using System.Reflection;

namespace Disc.NET.Enums
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

        public static TEnum? ToEnum<TEnum>(this string? value)
       where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var normalized = value.Trim();

            // Match direto (nome do enum)
            if (Enum.TryParse<TEnum>(normalized, true, out var directMatch))
                return directMatch;

            // Match por DescriptionAttribute
            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = field.GetCustomAttribute<DescriptionAttribute>();

                if (attr != null &&
                    string.Equals(attr.Description, normalized, StringComparison.OrdinalIgnoreCase))
                {
                    return (TEnum)field.GetValue(null)!;
                }
            }

            return null;
        }

        public static TEnum? ToEnum<TEnum>(this int? value)
       where TEnum : struct, Enum
        {
            if (value is null)
                return null;

            if (Enum.IsDefined(typeof(TEnum), value.Value))
                return (TEnum)Enum.ToObject(typeof(TEnum), value.Value);

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
