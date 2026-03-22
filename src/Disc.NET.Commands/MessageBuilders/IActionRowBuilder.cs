using Disc.NET.Client.SDK.Messages.Components;
using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands.MessageBuilders
{
    public interface IActionRowBuilder
    {
        public string? Id { get; }
        public List<object> Components { get; }
        object Build();

    }
}