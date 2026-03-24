using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Client.SDK.Messages.Components.Enums;
using Disc.NET.Client.SDK.Messages.Components.Selects;
using System.Text.Json.Serialization;

namespace Disc.NET.Client.SDK.Messages.Components
{
    [JsonDerivedType(typeof(ButtonComponent))]
    [JsonDerivedType(typeof(StringSelectComponent))]
    public interface IMessageComponent
    {
        MessageComponentType Type { get; }
        string? CustomId { get; set; }
    }
}
