using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disc.NET.Client.SDK.Messages.Components.Enums;

namespace Disc.NET.Client.SDK.Messages.Components.Selects
{
    public class StringSelectComponent
    {
        public MessageComponentType Type => MessageComponentType.SelectMenu;
        public int? Id { get; set; }
        public required string CustomId { get; set; }
	}
}
