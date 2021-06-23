using KeyPusher.Configuration;
using KeyPusher.Services;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace KeyPusher.Menus
{
    public class DisableHookMenuItem : MenuItemPresenterBase<DisableHookMenuItem>
    {
        public DisableHookMenuItem(KeyPusherEngine engine, ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<DisableHookMenuItem> logger) : base(engine, mainMenu, hotkeys, logger)
        {
            HotKeyCode = hotkeys.TurnHookOff;
        }

        public sealed override string ActionName => "Disable hooks";
        public sealed override byte? HotKeyCode { get; }
        protected sealed override void ExecuteInternal() => _engine.Enabled = false;
        protected override bool EnablementFunction() => _engine.Enabled = true;
    }
}
