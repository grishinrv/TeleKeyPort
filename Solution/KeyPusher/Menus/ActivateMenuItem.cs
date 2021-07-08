using KeyPusher.Configuration;
using KeyPusher.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace KeyPusher.Menus
{
    public class ActivateMenuItem : MenuItemPresenterBase<ActivateMenuItem>
    {
        public ActivateMenuItem(KeyPusherEngine engine, ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<ActivateMenuItem> logger) : base(engine, mainMenu, hotkeys, logger)
        {
            HotKeyCode = hotkeys.TurnHookOnOff;
        }

        public sealed override string ActionName => "Turn proxy on/off";
        public sealed override byte? HotKeyCode { get; }
        protected sealed override void ExecuteInternal()
        {
            _engine.Enabled = !_engine.Enabled;
        }

        protected sealed override bool EnablementFunction() => true;
    }
}
