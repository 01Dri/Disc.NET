using Disc.NET.Client.SDK.Messages.Components.Enums;
using System.Text.Json.Serialization;

namespace Disc.NET.Client.SDK.Messages.Components.Buttons
{
    public class ButtonComponent : IMessageComponent
    {
        public MessageComponentType Type  => MessageComponentType.Button;
        public string? Id { get; set; }
        public ButtonStyle Style { get; set; }
        public string? Label { get; set; }

        // emoji

        public string? CustomId { get; set; }
		public string? SkuId { get; set; }
		public string? Url { get; set; }

        public bool Disabled { get; set; }

        [JsonIgnore]
        public Func<bool> Callback { get; set; }

        public ButtonComponent(ButtonStyle style, string customId)
        {
			Style = style;
            CustomId = customId;
        }

    }
}
