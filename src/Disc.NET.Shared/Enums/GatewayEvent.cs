using System.ComponentModel;

namespace Disc.NET.Shared.Enums
{
    internal enum GatewayEvent
    {
        None,
        [Description("READ")]
        Ready,
        [Description("MESSAGE_CREATE")]
        MessageCreate,
        [Description("MESSAGE_DELETE")]
        MessageDelete,
        [Description("INTERACTION_CREATE")]
        InteractionCreate
    }
}
