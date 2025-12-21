using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Client.SDK.Messages.Components
{
    public sealed class ActionRowButtonComponentBuilder : IMessageComponentBuilder
    {
        public string? Id { get; }
        public List<object> Components { get; } = [];

        public ActionRowButtonComponentBuilder()
        {
        }

        public ActionRowButtonComponentBuilder(string? id)
        {
            Id = id;
        }

        public ActionRowButtonComponentBuilder AddButton(IMessageComponent component)
        {
            Components.Add(component);
            return this;
        }

        public ActionRowButtonComponentBuilder AddButtons(List<IMessageComponent> components)
        {
            Components.AddRange(components);
            return this;
        }
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
