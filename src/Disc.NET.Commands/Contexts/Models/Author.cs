using System.Text.Json.Serialization;

namespace Disc.NET.Commands.Contexts.Models
{
    public class Author
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("public_flags")]
        public int PublicFlags { get; set; }

        [JsonPropertyName("primary_guild")]
        public string? PrimaryGuild { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("global_name")]
        public string? GlobalName { get; set; }

        [JsonPropertyName("display_name_styles")]
        public object? DisplayNameStyles { get; set; }

        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; } = string.Empty;

        [JsonPropertyName("collectibles")]
        public object? Collectibles { get; set; }

        [JsonPropertyName("clan")]
        public object? Clan { get; set; }

        //[JsonPropertyName("avatar_decoration_data")]
        //public AvatarDecorationData? AvatarDecorationData { get; set; }

        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }
    }
}
