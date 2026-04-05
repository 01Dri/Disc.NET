using System.ComponentModel;

namespace Disc.NET.Shared.Constraints
{
    internal static class CallbackCustomIdHelper
    {
        public const string Separator = ":";

        public static CallbackCustomId GetCallbackCustomId(string guild, string customIdWithSuffix)
        {
            var customId = GetCustomId(customIdWithSuffix);
            var callbackType = GetCallbackType(customIdWithSuffix);
            return new CallbackCustomId(callbackType, BuildCustomId(guild, customId, callbackType));
        }
        public static string BuildCustomId(string guildId, string customId, CallbackType callback)
        {
            return guildId + Separator + customId + Separator + callback.GetCallbackSuffix();
        }

        public static string BuildCustomId(string customId, CallbackType callback)
        {
            return  customId + Separator + callback.GetCallbackSuffix();
        }

        public static string GetCustomId(string customId)
        {
            var parts = customId.Split(Separator);
            if (parts.Length >= 2)
            {
                return parts[parts.Length - 2];
            }
            return string.Empty;
        }

        internal static CallbackType GetCallbackType(string customId)
        {
            if (customId.EndsWith(CallbackType.PrefixCommand.GetCallbackSuffix()))
            {
                return CallbackType.PrefixCommand;
            }
            else if (customId.EndsWith(CallbackType.InteractionCommand.GetCallbackSuffix()))
            {
                return CallbackType.InteractionCommand;
            }
            return CallbackType.None;
        }

        internal static string GetCallbackSuffix(this CallbackType callback)
        {
            var type = typeof(CallbackType);
            var memberInfo = type.GetMember(callback.ToString());
            if (memberInfo.Length > 0)
            {
                var descriptionAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;
                if (descriptionAttribute != null)
                {
                    return descriptionAttribute.Description;
                }
            }
            return string.Empty;
        }
    }

    internal enum CallbackType
    {
        None,
        [Description("prefix-command")]
        PrefixCommand,
        [Description("interaction-command")]
        InteractionCommand
    }

    internal class CallbackCustomId
    {
        public CallbackType CallbackType { get; set; }
        public string Id { get; set; }

        public CallbackCustomId(CallbackType callbackType, string id)
        {
            CallbackType = callbackType;
            Id = id;
        }
    }
}
