using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands
{
    public interface IPrefixCommand : ICommand<CommandContext>
    {
        Task RunAsync(CommandContext context, CancellationToken cancellationToken = default);
    }
}