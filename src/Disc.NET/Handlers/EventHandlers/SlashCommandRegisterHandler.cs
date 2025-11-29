using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using System.Text.Json;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class SlashCommandRegisterHandler : HandlerCommandBase, IHandler
    {
        public SlashCommandRegisterHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public override async Task HandleAsync(GatewayEvent @event, JsonDocument contextJson,
            AppConfiguration configuration)
        {
            if (@event != GatewayEvent.Ready)
            {
                await base.HandleAsync(@event, contextJson, configuration).ConfigureAwait(false);
                return;
            }

            List<SlashCommandAttribute> attributes = GetCommandAttributes<SlashCommandAttribute>();
            foreach (var attribute in attributes)
            {
                var command = (ISlashCommand)
                    GetCommandByAttribute<SlashCommandAttribute, CommandContext>(attribute.Name);
                if (command == null)
                {
                    continue;
                }
                var commandObject = new SlashCommandCreate()
                {
                    Name = attribute.Name,
                    Description = attribute.Description,
                    Type = attribute.Type,
                    Options = command.BuildOptions()
                };
                var commandJson = Serializer.Serialize(commandObject);
                var guildId = attribute.GuildId;
                if (!string.IsNullOrEmpty(guildId))
                {
                    await Client.RegisterGuildSlashCommandAsync(commandJson, guildId).ConfigureAwait(false);
                    continue;
                }
                await Client.RegisterGlobalSlashCommandAsync(commandJson);
            }
        }
    }
}
