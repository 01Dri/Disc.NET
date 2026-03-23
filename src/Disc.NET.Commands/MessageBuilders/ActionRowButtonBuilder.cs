using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Client.SDK.Messages.Components.Enums;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.MessageBuilders
{
    public sealed class ActionRowButtonBuilder : ActiorRowBuilderBase, IActionRowBuilder
    {
        public string? Id { get; }
        public List<object> Components { get; } = [];

        public ActionRowButtonBuilder()
        {
        }

        public ActionRowButtonBuilder(string? id)
        {
            Id = id;
        }

        public IActionRowBuilder AddComponent<T>(IMessageComponent component, T? context = null, Func<T, Task>? callback = null) where T : ContextBase
        {
            if (context != null && callback != null)
            {
                if (string.IsNullOrEmpty(component.CustomId))
                    throw new ArgumentException("CustomId must be set for the component when a callback is provided.");

                RegisterComponentCallback(component, context, callback);
            }
            Components.Add(component);
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
