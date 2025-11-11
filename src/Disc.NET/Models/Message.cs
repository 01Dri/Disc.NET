using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Disc.NET.Models
{
    public class Message
    {

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

    }
}
