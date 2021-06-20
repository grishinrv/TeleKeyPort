using System;
using System.Windows.Forms;

namespace KeyPusher.Models
{
    /// <summary>
    /// Key live cycle event args.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(Keys key)
        {
            Key = key;
        }

        public Keys Key { get; }
    }
}
