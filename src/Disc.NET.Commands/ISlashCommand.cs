using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands
{
    public interface ISlashCommand : ICommand<InteractionContext>
    {
        Task RunAsync(InteractionContext context, CancellationToken cancellation = default);
    }
}
