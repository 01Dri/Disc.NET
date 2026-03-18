using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Client.SDK.Messages.Components.Selects
{
    public class StringSelectComponent : ISelectComponent
    {
        public MessageComponentType Type => MessageComponentType.SelectMenu;

        public int? Id { get; set; }

        public required string CustomId { get; set; }

        public List<StringSelectOption> Options { get; set; } = [];

        public string? Placeholder { get; set; }
        public int? MinValues { get; set; }
        public int? MaxValues { get; set; }
        public bool? Disabled { get; set; }
        public Func<bool> Callback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class StringSelectOption
    {
        public required string Label { get; set; }
        public required string Value { get; set; }

        public string? Description { get; set; }
        public bool Default { get; set; }
    }
}
