using Disc.NET.Commands.Contexts;

namespace Disc.NET.Commands
{
    public interface ISlashCommand : ICommand<InteractionContext>
    {
        Task RunAsync(InteractionContext context, SlashCommandParamsResult @params);
        List<SlashCommandOptions> BuildOptions() => [];

    }
}
