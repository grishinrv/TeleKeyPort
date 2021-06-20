using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

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
