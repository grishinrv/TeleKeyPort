using KeyPusher.Configuration;
using KeyPusher.Models;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace KeyPusher.Menus
{
    public class DisableHookMenuItem : MenuItemPresenterBase<DisableHookMenuItem>
    {
        public DisableHookMenuItem(ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<DisableHookMenuItem> logger) : base(mainMenu, hotkeys, logger)
        {
        }

        protected sealed override string ActionName { get; }
        internal sealed override byte? HotKeyCode { get; }
        protected sealed override void ExecuteInternal()
        {
            //todo
        }

        protected override bool EnablementFunction(MenuController menuController) => menuController.AppMode == Mode.Active;
    }
}
