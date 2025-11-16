using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disc.NET.Client.SDK.Messages
{
    public class ApiMessage
    {
        public string Content { get; set; } = string.Empty;
        public List<Embed> Embeds { get; set; } = [];

    }
}
