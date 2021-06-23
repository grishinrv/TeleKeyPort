using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using KeyPusher.Configuration;
using KeyPusher.Models;
using KeyPusher.Services;
using Microsoft.Extensions.Logging;

namespace KeyPusher.Menus
{
    public class EnableHookMenuItem : MenuItemPresenterBase<EnableHookMenuItem>
    {
        public EnableHookMenuItem(ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<EnableHookMenuItem> logger) : base(mainMenu, hotkeys, logger)
        {
        }

        protected sealed override string ActionName => "Enable hooks";
        internal sealed override byte? HotKeyCode => null;
        protected sealed override void ExecuteInternal()
        {
            //todo
        }

        protected sealed override bool EnablementFunction(MenuController menuController) => menuController.AppMode == Mode.Ignore;
    }
}
