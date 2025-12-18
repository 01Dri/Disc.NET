using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Client.SDK.Messages.Components
{
    public class ActionRowComponent
    {
        public MessageComponentType Type { get => MessageComponentType.ActionRow; }
        public List<object> Components { get; set; } = [];
    }

}
