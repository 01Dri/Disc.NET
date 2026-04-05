namespace Disc.NET.Commands.Responses
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResponse
    {
        Task SendMessageAsync(Message message, CancellationToken cancellation = default);
        Task ReplyAsync(Message message, bool isInteraction = false,  CancellationToken cancellation = default);

        Task SendInteractionResponseAsync(Message message, bool ephemeral = false,
            CancellationToken cancellation = default);
    }
}
