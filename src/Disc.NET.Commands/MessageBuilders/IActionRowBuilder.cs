using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Client.SDK.Messages.Components.Selects;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.MessageBuilders
{
    public interface IActionRowBuilder
    {
        public List<IMessageComponent> Components { get; }
        object Build();
        IActionRowBuilder AddComponent(IMessageComponent component);
        IActionRowBuilder AddComponent<T>(IMessageComponent component, T? context = null, Func<T, Task>? callback = null) where T : ContextBase;
        IActionRowBuilder AddButton(ButtonComponent button);
        IActionRowBuilder AddButton<T>(ButtonComponent button, T? context = null, Func<T, Task>? callback = null) where T : ContextBase;
        IActionRowBuilder AddSelectMenu(StringSelectComponent selectMenu);
        IActionRowBuilder AddSelectMenu<T>(StringSelectComponent selectMenu, T? context = null, Func<T, Task>? callback = null) where T : ContextBase;
    }
}
