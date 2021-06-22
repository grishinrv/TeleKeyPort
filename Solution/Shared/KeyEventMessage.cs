using MessagePack;

namespace Shared
{
    [MessagePackObject]
    public class KeyEventMessage
    {
        [Key(0)]
        public int KeyCode { get; set; }
        [Key(1)]
        public uint EventCode { get; set; }
    }
}
