using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Client.SDK.Messages.Components.Builders.ComponentBuilders
{
    public class ActionRowComponentBuilder : IMessageComponentBuilder
    {
        public string? Id { get; }

        public ActionRowComponentBuilder(string? id)
        {
            Id = id;
        }

        private List<object> _components = new List<object>();
        public ActionRowComponentBuilder AddButton(ButtonComponent buttonComponent)
        {
            _components.Add(buttonComponent);
            return this;
        }

  //      public ActionRowComponentBuilder Add
  //      {
  //          _components.AddRange(components);
  //          return this;
		//}

		public object Build()
        {
            return new
            {
                id = Id,
                type = MessageComponentType.ActionRow,
                components = _components
            };
        }
    }
}
