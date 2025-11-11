using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Disc.NET.Models
{
    public class Channel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("guild_id")]
        public ulong? GuildId { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; }

        //[JsonPropertyName("permission_overwrites")]
        //public List<PermissionOverwrite>? PermissionOverwrites { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("topic")]
        public string? Topic { get; set; }

        [JsonPropertyName("nsfw")]
        public bool? Nsfw { get; set; }

        [JsonPropertyName("last_message_id")]
        public ulong? LastMessageId { get; set; }

        [JsonPropertyName("bitrate")]
        public int? Bitrate { get; set; }

        [JsonPropertyName("user_limit")]
        public int? UserLimit { get; set; }

        [JsonPropertyName("rate_limit_per_user")]
        public int? RateLimitPerUser { get; set; }

        //[JsonPropertyName("recipients")]
        //public List<User>? Recipients { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("owner_id")]
        public ulong? OwnerId { get; set; }

        [JsonPropertyName("application_id")]
        public ulong? ApplicationId { get; set; }

        [JsonPropertyName("managed")]
        public bool? Managed { get; set; }

        [JsonPropertyName("parent_id")]
        public ulong? ParentId { get; set; }

        [JsonPropertyName("last_pin_timestamp")]
        public DateTimeOffset? LastPinTimestamp { get; set; }

        [JsonPropertyName("rtc_region")]
        public string? RtcRegion { get; set; }

        [JsonPropertyName("video_quality_mode")]
        public int? VideoQualityMode { get; set; }

        [JsonPropertyName("message_count")]
        public int? MessageCount { get; set; }

        [JsonPropertyName("member_count")]
        public int? MemberCount { get; set; }

        //[JsonPropertyName("thread_metadata")]
        //public ThreadMetadata? ThreadMetadata { get; set; }

        //[JsonPropertyName("member")]
        //public ThreadMember? Member { get; set; }

        [JsonPropertyName("default_auto_archive_duration")]
        public int? DefaultAutoArchiveDuration { get; set; }

        [JsonPropertyName("permissions")]
        public string? Permissions { get; set; }

        [JsonPropertyName("flags")]
        public int? Flags { get; set; }

        [JsonPropertyName("total_message_sent")]
        public int? TotalMessageSent { get; set; }

        //[JsonPropertyName("available_tags")]
        //public List<Tag>? AvailableTags { get; set; }

        [JsonPropertyName("applied_tags")]
        public List<ulong>? AppliedTags { get; set; }

        //[JsonPropertyName("default_reaction_emoji")]
        //public DefaultReaction? DefaultReactionEmoji { get; set; }

        [JsonPropertyName("default_thread_rate_limit_per_user")]
        public int? DefaultThreadRateLimitPerUser { get; set; }

        [JsonPropertyName("default_sort_order")]
        public int? DefaultSortOrder { get; set; }

        [JsonPropertyName("default_forum_layout")]
        public int? DefaultForumLayout { get; set; }
    }
}
