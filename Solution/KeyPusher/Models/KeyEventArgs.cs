using System;
using System.Windows.Forms;

namespace KeyPusher.Models
{
    /// <summary>
    /// Key live cycle event args.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(Keys key, int eventCode)
        {
            Key = key;
            EventCode = eventCode;
        }
        public Keys Key { get; }
        public int EventCode { get; }
    }
}
