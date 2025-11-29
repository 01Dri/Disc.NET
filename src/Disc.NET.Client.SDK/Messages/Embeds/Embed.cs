namespace Disc.NET.Client.SDK.Messages.Embeds
{
    public class Embed
    {
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Timestamp { get; set; }
        public int? Color { get; set; }
        public EmbedFooter? Footer { get; set; }
        public EmbedImage? Image { get; set; }
        public EmbedImage? Thumbnail { get; set; }

        public EmbedVideo? Video { get; set; }

        public EmbedProvider? Provider { get; set; }
        public EmbedAuthor? Author { get; set; }
        public List<EmbedField>? Fields { get; set; }

    }

}
