using KeyPusher.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace KeyPusher.Menus
{
    public abstract class MenuItemPresenterBase<T> : IMenuItemPresenter where T : MenuItemPresenterBase<T>
    {
        protected readonly ILogger<T> _logger;
        protected abstract string ActionName { get; }
        private readonly ToolStripItem _menuItem;
        internal abstract byte? HotKeyCode { get; }
        protected MenuItemPresenterBase(ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<T> logger)
        {
            _logger = logger;
            _menuItem = mainMenu.Items.Add(ActionName);
        }

        public void ExecuteAction()
        {
            try
            {
                ExecuteInternal();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to execute menu action {0}", ActionName);
            }
        }

        public void StateChanged(MenuController menuController) => _menuItem.Enabled = EnablementFunction(meniController);

        protected abstract void ExecuteInternal();

        protected virtual bool EnablementFunction(MenuController menuController) => true;
    }
}
