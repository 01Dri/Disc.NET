using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Configuration;
using Disc.NET.Dispatcher;
using Disc.NET.Enums;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class SlashCommandRegisterHandler : HandlerCommandBase, IHandler
    {
        public SlashCommandRegisterHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public GatewayEvent GetEventType()
            => GatewayEvent.Ready;

        public async Task HandleAsync(EventHandlerPayload payload, AppConfiguration configuration)
        {
            List<SlashCommandAttribute> attributes = GetCommandAttributes<SlashCommandAttribute>();
            foreach (var attribute in attributes)
            {
                var command = (ISlashCommand)
                    GetCommandByAttribute<SlashCommandAttribute, InteractionContext>(attribute.Name);
                if (command == null)
                {
                    continue;
                }
                var slashCommandToRegisterObject = new SlashCommandCreate()
                {
                    Name = attribute.Name,
                    Description = attribute.Description,
                    Type = attribute.Type,
                };
                var slashComamndToRegisterJson = Serializer.Serialize(slashCommandToRegisterObject);
                var guildId = attribute.GuildId;
                if (!string.IsNullOrEmpty(guildId))
                {
                    await Client.RegisterGuildSlashCommandAsync(slashComamndToRegisterJson, guildId).ConfigureAwait(false);
                    continue;
                }
                await Client.RegisterGlobalSlashCommandAsync(slashComamndToRegisterJson);
            }
        }
    }
}
