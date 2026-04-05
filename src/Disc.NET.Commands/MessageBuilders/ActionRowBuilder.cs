using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Client.SDK.Messages.Components.Enums;
using Disc.NET.Client.SDK.Messages.Components.Selects;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.MessageBuilders
{
    public sealed class ActionRowBuilder : ActionRowBuilderBase, IActionRowBuilder
    {
        public List<IMessageComponent> Components { get; } = [];

        public ActionRowBuilder()
        {
        }

        public IActionRowBuilder AddComponent(IMessageComponent component)
        {
            Components.Add(component);
            return this;
        }

        public IActionRowBuilder AddComponent<T>(IMessageComponent component, T? context = null, Func<T, Task>? callback = null) where T : ContextBase
        {
            if (context != null && callback != null)
            {
                if (string.IsNullOrEmpty(component.CustomId))
                    throw new ArgumentException("CustomId must be set for the component when a callback is provided.");

                RegisterComponentCallback(component, context, callback);
            }
            return AddComponent(component);
        }

        public IActionRowBuilder AddButton(ButtonComponent button)
        {
            return AddComponent(button);
        }

        public IActionRowBuilder AddButton<T>(ButtonComponent button, T? context = null, Func<T, Task>? callback = null) where T : ContextBase
        {
            return AddComponent(button, context, callback);
        }

        public IActionRowBuilder AddSelectMenu(StringSelectComponent selectMenu)
        {
            return AddComponent(selectMenu);
        }

        public IActionRowBuilder AddSelectMenu<T>(StringSelectComponent selectMenu, T? context = null, Func<T, Task>? callback = null) where T : ContextBase
        {
            return AddComponent(selectMenu, context, callback);
        }

        public object Build()
        {
            return new
            {
                Type = MessageComponentType.ActionRow,
                Components = Components
            };
        }
    }
}
