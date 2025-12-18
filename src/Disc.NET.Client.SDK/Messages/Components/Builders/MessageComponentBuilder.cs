using Disc.NET.Client.SDK.Messages.Components.Builders.ComponentBuilders;

namespace Disc.NET.Client.SDK.Messages.Components.Builders
{
    public class MessageComponentBuilder
    {
        public static ActionRowComponentBuilder WithActionRow(string? id = null) => new ActionRowComponentBuilder(id);

    }
}
