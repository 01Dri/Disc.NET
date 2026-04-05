using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Client.SDK.Messages.Components.Selects;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.MessageBuilders
{
    public static class ActionRowBuilderExtensions
    {
        public static IActionRowBuilder AddButton(this IActionRowBuilder builder, string label, string customId, ButtonStyle style = ButtonStyle.Primary)
        {
            var button = new ButtonComponent(style)
            {
                Label = label,
                CustomId = customId,
            };
            return builder.AddButton(button);
        }

        public static IActionRowBuilder AddButton<T>(this IActionRowBuilder builder, string label, string customId, ButtonStyle style = ButtonStyle.Primary, T? context = null, Func<T, Task>? callback = null) where T : ContextBase
        {
            var button = new ButtonComponent(style)
            {
                Label = label,
                CustomId = customId,
            };
            return builder.AddButton(button, context, callback);
        }

        public static IActionRowBuilder AddLinkButton(this IActionRowBuilder builder, string label, string url)
        {
            var button = new ButtonComponent(ButtonStyle.Link)
            {
                Label = label,
                Url = url,
            };
            return builder.AddButton(button);
        }

        public static IActionRowBuilder AddSelectMenu(this IActionRowBuilder builder, string customId, List<StringSelectOption> options, string? placeholder = null)
        {
            var selectMenu = new StringSelectComponent
            {
                CustomId = customId,
                Options = options,
                Placeholder = placeholder
            };
            return builder.AddSelectMenu(selectMenu);
        }

        public static IActionRowBuilder AddSelectMenu<T>(this IActionRowBuilder builder, string customId, List<StringSelectOption> options, string? placeholder = null, T? context = null, Func<T, Task>? callback = null) where T : ContextBase
        {
            var selectMenu = new StringSelectComponent
            {
                CustomId = customId,
                Options = options,
                Placeholder = placeholder
            };
            return builder.AddSelectMenu(selectMenu, context, callback);
        }
    }
}
