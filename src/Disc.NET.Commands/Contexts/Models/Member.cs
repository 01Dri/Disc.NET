using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disc.NET.Commands.Contexts.Models
{
    public class Member
    {
        public List<string> Roles { get; set; } = [];
        public DateTime? PremiumSince { get; set; }
        public bool Pending { get; set; }
        public string? Nick { get; set; }
        public bool Mute { get; set; }
        public DateTime JoinedAt { get; set; }
        public int Flags { get; set; }
        public bool Deaf { get; set; }
        public DateTime? CommunicationDisabledUntil { get; set; }
        public string? Banner { get; set; }
        public string? Avatar { get; set; }

        public User? User { get; set; }
    }

    public class User
    {
        public string Username { get; set; } = string.Empty;
        public int PublicFlags { get; set; }
        public string? PrimaryGuild { get; set; }
        public string Id { get; set; } = string.Empty;
        public string GlobalName { get; set; } = string.Empty;
        public object? DisplayNameStyles { get; set; }
        public string Discriminator { get; set; } = string.Empty;
        public object? Collectibles { get; set; }
        public object? Clan { get; set; }
        public AvatarDecorationData? AvatarDecorationData { get; set; }
        public string Avatar { get; set; } = string.Empty;
    }

    public class AvatarDecorationData
    {
        public string SkuId { get; set; } = string.Empty;
        public long ExpiresAt { get; set; } // epoch time (int64)
        public string Asset { get; set; } = string.Empty;
    }


}
