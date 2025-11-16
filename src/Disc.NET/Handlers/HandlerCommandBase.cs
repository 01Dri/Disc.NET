using Disc.NET.Commands;
using Disc.NET.Commands.Contexts;
using Disc.NET.Commands.Contexts.Models;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Disc.NET.Handlers
{
    internal abstract class HandlerCommandBase : HandlerBase<CommandContext>
    {
        public HandlerCommandBase(AppConfiguration appConfiguration) : base(appConfiguration)
        {
        }

        protected ICommand<TK>? GetCommandByAttribute<T, TK>(string commandName, AppConfiguration configuration)
            where T : Attribute where TK : IContext
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                throw new InvalidOperationException("Could not determine the entry assembly.");

            var nameProperty = typeof(T).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
            if (nameProperty == null)
                throw new InvalidOperationException($"Attribute {typeof(T).Name} must have a public property called 'Name'.");

            var commandType = assembly
                .GetTypes()
                .Where(t => typeof(ICommand<TK>).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .FirstOrDefault(t =>
                {
                    var attr = t.GetCustomAttribute<T>();
                    if (attr == null) return false;

                    var nameValue = nameProperty.GetValue(attr) as string;
                    return nameValue != null &&
                           nameValue.Equals(commandName, StringComparison.OrdinalIgnoreCase);
                });
            // Temporally code I need other way to get singleton instance of CommandBase
            CommandBase.GetInstance(configuration);
            return commandType != null ? (ICommand<TK>)Activator.CreateInstance(commandType)! : null;
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


    }

    internal class CommandModel
    {
        public char? Prefix { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Params { get; set; } = [];

    }
}
