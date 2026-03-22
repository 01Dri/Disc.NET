using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Commands.MessageBuilders
{
    public sealed class ActionRowSelectMenuBuilder : ActiorRowBuilderBase, IActionRowBuilder
    {

        public ActionRowSelectMenuBuilder()
        {
        }

        public ActionRowSelectMenuBuilder(string? id)
        {
            Id = id;
        }

        public ActionRowSelectMenuBuilder AddMenu(IMessageComponent component)
        {
            Components.Add(component);
            return this;
        }


        public string? Id { get; set; }
        public List<object> Components { get; } = [];

        public object Build()
        {
            return new
            {
                id = Id,
                type = MessageComponentType.ActionRow,
                components = Components
            };
        }
    }

}
