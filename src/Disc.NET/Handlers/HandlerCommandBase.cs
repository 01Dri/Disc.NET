using Autofac;
using Disc.NET.Commands;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Commands.Responses;
using Disc.NET.Configuration;
using Disc.NET.Dispatcher;
using Disc.NET.Shared.Constraints;
using Disc.NET.Shared.Extensions;
using System.Reflection;
using System.Text.Json;

namespace Disc.NET.Handlers
{
    internal abstract class HandlerCommandBase : HandlerBase<CommandContext>
    {

        private readonly AppConfiguration _appConfiguration;
        public HandlerCommandBase(AppConfiguration appConfiguration) : base(appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        protected ICommand<TKContext>? GetCommandByAttribute<TAttribute, TKContext>(string commandName)
            where TKContext : ContextBase
            where TAttribute : Attribute

        {
            var commandType = GetCommandTypeByName<TAttribute, TKContext>(commandName);

            if (commandType == null) return null;
            return DiscNetContainer.GetInstance().Resolve(commandType) as ICommand<TKContext>;
            //return Activator.CreateInstance(commandType) as ICommand<TKContext>;
        }

        protected List<T> GetCommandAttributes<T>() where T : Attribute
        {   
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                throw new InvalidOperationException("Could not determine the entry assembly.");

            return assembly
                .GetTypes()
                .Where(t => t.GetCustomAttribute<T>() != null)
                .Select(t => t.GetCustomAttribute<T>()!)
                .ToList();
        }

        protected ContextBase BuildContextByCustomIdCallbackType(EventHandlerPayload payload, CallbackType callbackType, AppConfiguration appConfiguration)
        {
            if (callbackType == CallbackType.PrefixCommand)
            {
                return BuildCommandContext(payload.Data, appConfiguration);
            }
            return BuildInteractionContext(payload.Data, appConfiguration);
        }
        protected override CommandContext BuildCommandContext(JsonDocument contextJson, AppConfiguration appConfiguration)
        {
            CommandContext context = new CommandContext();
            var authorProperty = contextJson.GetJsonStringProperty("author");
            if (authorProperty != null)
            {
                context.Author = Serializer.Deserialize<Author>(authorProperty);
            }

            context.ChannelId = contextJson.GetStringProperty("channel_id") ?? string.Empty;
            context.Content = contextJson.GetStringProperty("content") ?? string.Empty;
            context.Id = contextJson.GetStringProperty("id") ?? string.Empty;
            context.ChannelType = contextJson.GetIntProperty("channel_type") ?? 0;
            context.GuildId = contextJson.GetStringProperty("guild_id") ?? string.Empty;
            context.Timestamp = contextJson.GetDateTimeProperty("timestamp");
            context.EditedTimestamp = contextJson.GetDateTimeProperty("edited_timestamp");
            context.Type = contextJson.GetIntProperty("type") ?? 0;
            context.Response = new CommandResponse()
            {
                ChannelId = context.ChannelId,
                MessageId = context.Id
            };
            return context;
        }

        protected override InteractionContext BuildInteractionContext(JsonDocument contextJson, AppConfiguration appConfiguration)
        {
            InteractionContext context = new InteractionContext();
            var memberProperty = contextJson.GetJsonStringProperty("member");
            if (memberProperty != null)
            {
                context.Member = Serializer.Deserialize<Member>(memberProperty);
            }

            context.Id = contextJson.GetStringProperty("id") ?? string.Empty;
            context.GuildId = contextJson.GetStringProperty("guild_id") ?? string.Empty;
            var channelProperty = contextJson.GetJsonStringProperty("channel");

            if (channelProperty != null)
            {
                context.Channel = Serializer.Deserialize<Channel>(channelProperty);
            }

            context.Type = contextJson.GetIntProperty("type") ?? 0;
            context.Context = contextJson.GetIntProperty("context") ?? 0;
            context.Token = contextJson.GetStringProperty("token") ?? string.Empty;
            context.Response = new InteractionResponse()
            {
                ChannelId = context.Channel?.Id ?? string.Empty,
                InteractionId = context.Id,
                InteractionToken = context.Token
            };

            var data = contextJson.GetJsonStringProperty("data");
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    context.Data = Serializer.Deserialize<InteractionData>(data);
                }
                catch (JsonException)
                {
                    context.Data = null;
                }
            }
            return context;
        }


        protected CommandModel BuildCommandModelByEventContent(string content)
        {
            var commandModel = new CommandModel();
            var prefix = content[0];
            var commandName = content[1..].Split(' ')[0].Trim().ToLowerInvariant();
            var contentAfterCommandName = content.Split(prefix + commandName);
            if (contentAfterCommandName.Length > 1)
            {
                var paramsString = contentAfterCommandName[1].Trim();
                if (!string.IsNullOrEmpty(paramsString))
                {
                    var paramsArray = paramsString.Split(' ');
                    commandModel.Params = paramsArray
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToList();
                }
            }

            commandModel.Name = commandName;
            commandModel.Prefix = prefix;
            return commandModel;
        }


        private Type? GetCommandTypeByName<T, TK>(string name)
        where TK : ContextBase
        where T : Attribute
        {
            var assembly = Assembly.GetEntryAssembly()
                           ?? throw new InvalidOperationException("Could not determine the entry assembly.");

            var nameProp = typeof(T).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance)
                           ?? throw new InvalidOperationException(
                               $"Attribute {typeof(T).Name} must have a public property called 'Name'.");

            var commandType = assembly
                .GetTypes()
                .Where(t =>
                    typeof(ICommand<TK>).IsAssignableFrom(t) &&
                    !t.IsAbstract &&
                    !t.IsInterface)
                .FirstOrDefault(t =>
                {
                    var attrs = t.GetCustomAttributes<T>();
                    foreach (var attr in attrs)
                    {
                        var nameValue = nameProp.GetValue(attr) as string;
                        if (nameValue != null &&
                            nameValue.Equals(name, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }

                    return false;
                });
            return commandType;

        }

    }

    internal class CommandModel
    {
        public char? Prefix { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Params { get; set; } = [];

    }
}
