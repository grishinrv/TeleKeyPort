using MessagePack;

namespace Shared
{
    [MessagePackObject]
    public class KeyEventMessage
    {
        [Key(0)]
        public int KeyCode { get; set; }
    }
}
