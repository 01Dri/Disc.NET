using System.Reflection;
using System.Text.Json;
using Disc.NET.Client.SDK;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using Disc.NET.Shared.Extensions;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class PrefixCommandHandler : HandlerBase<CommandContext>, IHandler
    {
        public PrefixCommandHandler(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        public override async Task HandleAsync(GatewayEvent @event, JsonDocument contextJson, AppConfiguration configuration)
        { 
            if (@event != GatewayEvent.MessageCreate)
            {
                await base.HandleAsync(@event, contextJson, configuration);
                return;
            }

            var content = contextJson.GetContent();

            if (content[0] != configuration.BotPrefix)
            {
                await base.HandleAsync(@event, contextJson, configuration);
                return;
            }
            var commandName = content[1..].Split(' ')[0].Trim().ToLowerInvariant();

            ICommand<CommandContext>? command = GetCommandByAttribute<PrefixCommandAttribute, CommandContext>(commandName, configuration);

            if (command == null)
            {
                await base.HandleAsync(@event, contextJson, configuration);
                return;
            }

            var context = BuildContext(contextJson);
            await command.RunAsync(context);
        }


        protected override CommandContext BuildContext(JsonDocument contextJson)
        {
            CommandContext context = new CommandContext();
            var authorProperty = contextJson.GetAuthor();
            context.Author = JsonSerializer.Deserialize<Author>(authorProperty);
            context.Channel = new Channel()
            {
                Id = contextJson.GetChannelId()
            };
            context.Message = new Message()
            {
                Content = contextJson.GetContent()
            };
            return context;
        }

    }
}
