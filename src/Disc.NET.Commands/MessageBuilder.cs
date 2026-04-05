using Disc.NET.Client.SDK.Messages.Embeds;
using Disc.NET.Commands.MessageBuilders;

namespace Disc.NET.Commands
{
    public class MessageBuilder
    {
        private readonly Message _message = new();

        public MessageBuilder WithContent(string content)
        {
            _message.Content = content;
            return this;
        }

        public MessageBuilder WithEmbed(Embed embed)
        {
            _message.AddEmbed(embed);
            return this;
        }

        public MessageBuilder WithEmbed(EmbedBuilder embedBuilder)
        {
            _message.AddEmbed(embedBuilder);
            return this;
        }

        public MessageBuilder WithEmbed(Action<EmbedBuilder> configure)
        {
            _message.AddEmbed(configure);
            return this;
        }

        public MessageBuilder WithActionRow(IActionRowBuilder actionRowBuilder)
        {
            _message.ActionRows.Add(actionRowBuilder);
            return this;
        }

        public Message Build()
        {
            return _message;
        }
    }
}
