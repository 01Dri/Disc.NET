using System.Reflection;
using System.Text.Json;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Commands.Interfaces;
using Disc.NET.Configurations;
using Disc.NET.Enums;
using Disc.NET.Extensions;

namespace Disc.NET.Handlers.EventHandlers
{
    internal class PrefixCommandHandler : HandlerBase<CommandContext>, IHandler
    {

        public PrefixCommandHandler()
        {
        }

        public override async Task HandleAsync(GatewayEvent @event, JsonDocument contextJson, AppOptions options)
        { 
            if (@event != GatewayEvent.MessageCreate)
            {
                await base.HandleAsync(@event, contextJson, options);
                return;
            }

            var content = contextJson.GetContent();

            if (content[0] != options.BotPrefix)
            {
                await base.HandleAsync(@event, contextJson, options);
                return;
            }
            var commandName = content[1..].Split(' ')[0].Trim().ToLowerInvariant();

            ICommand<CommandContext>? command = GetCommandByAttribute<PrefixCommandAttribute, CommandContext>(commandName);

            if (command == null)
            {
                await base.HandleAsync(@event, contextJson, options);
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
