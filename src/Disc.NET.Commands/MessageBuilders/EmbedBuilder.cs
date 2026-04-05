using Disc.NET.Client.SDK.Messages.Embeds;

namespace Disc.NET.Commands.MessageBuilders
{
    public sealed class EmbedBuilder
    {
        private readonly Embed _embed = new();

        public EmbedBuilder SetTitle(string? title)
        {
            _embed.Title = title;
            return this;
        }

        public EmbedBuilder SetDescription(string? description)
        {
            _embed.Description = description;
            return this;
        }

        public EmbedBuilder SetType(string? type)
        {
            _embed.Type = type;
            return this;
        }

        public EmbedBuilder SetUrl(string? url)
        {
            _embed.Url = url;
            return this;
        }

        public EmbedBuilder SetTimestamp(DateTimeOffset timestamp)
        {
            _embed.Timestamp = timestamp.ToString("O");
            return this;
        }

        public EmbedBuilder SetTimestamp(string? timestamp)
        {
            _embed.Timestamp = timestamp;
            return this;
        }

        public EmbedBuilder SetColor(int? color)
        {
            _embed.Color = color;
            return this;
        }

        public EmbedBuilder SetFooter(string text, string? iconUrl = null, string? proxyIconUrl = null)
        {
            _embed.Footer = new EmbedFooter
            {
                Text = text,
                IconUrl = iconUrl,
                ProxyIconUrl = proxyIconUrl,
            };

            return this;
        }

        public EmbedBuilder SetImage(string url)
        {
            _embed.Image = new EmbedImage
            {
                Url = url,
            };

            return this;
        }

        public EmbedBuilder SetThumbnail(string url)
        {
            _embed.Thumbnail = new EmbedImage
            {
                Url = url,
            };

            return this;
        }

        public EmbedBuilder SetAuthor(string name, string? url = null, string? iconUrl = null, string? proxyIconUrl = null)
        {
            _embed.Author = new EmbedAuthor
            {
                Name = name,
                Url = url,
                IconUrl = iconUrl,
                ProxyIconUrl = proxyIconUrl,
            };

            return this;
        }

        public EmbedBuilder AddField(string name, string value, bool inline = false)
        {
            _embed.Fields ??= [];
            _embed.Fields.Add(new EmbedField
            {
                Name = name,
                Value = value,
                Inline = inline,
            });

            return this;
        }

        public EmbedBuilder ClearFields()
        {
            _embed.Fields?.Clear();
            return this;
        }

        public Embed Build()
        {
            return _embed;
        }
    }
}
