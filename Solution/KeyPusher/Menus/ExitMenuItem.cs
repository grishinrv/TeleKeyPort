using KeyPusher.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace KeyPusher.Menus
{
    public class ExitMenuItem : MenuItemPresenterBase<ExitMenuItem>
    {
        public ExitMenuItem(ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<ExitMenuItem> logger) : base(mainMenu, hotkeys, logger)
        {
        }

        protected sealed override string ActionName => "Exit";
        internal sealed override byte? HotKeyCode => null;
        protected sealed override void ExecuteInternal() => _state.Dispose();
    }
}
