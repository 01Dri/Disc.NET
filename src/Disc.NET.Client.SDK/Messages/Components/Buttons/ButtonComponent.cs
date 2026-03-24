using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Client.SDK.Messages.Components.Buttons
{
    public class ButtonComponent : IMessageComponent
    {
        public MessageComponentType Type  => MessageComponentType.Button;
        public string? Id { get; set; }
        public ButtonStyle Style { get; set; }
        public string? Label { get; set; }

        // emoji

        public required string CustomId { get; set; }
		public string? SkuId { get; set; }
		public string? Url { get; set; }

        public bool Disabled { get; set; }

        public ButtonComponent(ButtonStyle style)
        {
			Style = style;
        }

    }
}
