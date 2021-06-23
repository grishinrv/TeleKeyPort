using MessagePack;

namespace Shared.Models
{
    [MessagePackObject]
    public class KeyEventMessage
    {
        [Key(0)]
        public byte KeyCode { get; set; }
        [Key(1)]
        public uint EventCode { get; set; }
    }
}
