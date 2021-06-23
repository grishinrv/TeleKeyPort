using KeyPusher.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;
using KeyPusher.Services;

namespace KeyPusher.Menus
{
    public class ExitMenuItem : MenuItemPresenterBase<ExitMenuItem>
    {
        public ExitMenuItem(KeyPusherEngine engine, ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<ExitMenuItem> logger) : base(engine, mainMenu, hotkeys, logger)
        {
        }

        public sealed override string ActionName => "Exit";
        public sealed override byte? HotKeyCode => null;
        protected sealed override void ExecuteInternal() => _engine.Dispose();
    }
}
