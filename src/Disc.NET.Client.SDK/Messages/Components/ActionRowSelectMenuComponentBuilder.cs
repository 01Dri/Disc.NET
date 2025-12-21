using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Client.SDK.Messages.Components
{
    public sealed class ActionRowSelectMenuComponentBuilder : IMessageComponentBuilder
    {

        public ActionRowSelectMenuComponentBuilder()
        {
        }

        public ActionRowSelectMenuComponentBuilder(string? id)
        {
            Id = id;
        }

        public ActionRowSelectMenuComponentBuilder AddMenu(IMessageComponent component)
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
