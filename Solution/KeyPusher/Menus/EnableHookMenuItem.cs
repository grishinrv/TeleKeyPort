using KeyPusher.Configuration;
using KeyPusher.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace KeyPusher.Menus
{
    public class EnableHookMenuItem : MenuItemPresenterBase<EnableHookMenuItem>
    {
        public EnableHookMenuItem(KeyPusherEngine engine, ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<EnableHookMenuItem> logger) : base(engine, mainMenu, hotkeys, logger)
        {
            HotKeyCode = hotkeys.TurnHookOn;
        }

        public sealed override string ActionName => "Enable hooks";
        public sealed override byte? HotKeyCode { get; }
        protected sealed override void ExecuteInternal() => _engine.Enabled = true;
        protected sealed override bool EnablementFunction() => !_engine.Enabled;
    }
}
