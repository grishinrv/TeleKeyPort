using KeyPusher.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;
using KeyPusher.Services;

namespace KeyPusher.Menus
{
    public abstract class MenuItemPresenterBase : IMenuItemPresenter
    {
        protected readonly KeyPusherEngine _engine;
        public abstract string ActionName { get; }
        private ToolStripItem MenuItem { get; set; }
        private readonly ContextMenuStrip _mainMenu;
        public abstract byte? HotKeyCode { get; }
        protected MenuItemPresenterBase(KeyPusherEngine engine, ContextMenuStrip mainMenu, HotKeysOptions hotkeys)
        {
            _engine = engine;
            _mainMenu = mainMenu;
        }

        public abstract void ExecuteAction();

        public void StateChanged()
        {
            if (_mainMenu.InvokeRequired)
                _mainMenu.Invoke(new Action(() => { MenuItem.Enabled = EnablementFunction(); }));
            else
                MenuItem.Enabled = EnablementFunction();
        }

        public void CreateView()
        {
            MenuItem = _mainMenu.Items.Add(ActionName);
            StateChanged();
        }

        protected abstract void ExecuteInternal();

        protected virtual bool EnablementFunction() => true;
    }

    public abstract class MenuItemPresenterBase<T> : MenuItemPresenterBase where T : MenuItemPresenterBase<T>
    {
        protected readonly ILogger<T> _logger;
        protected MenuItemPresenterBase(KeyPusherEngine engine, ContextMenuStrip mainMenu, HotKeysOptions hotkeys, ILogger<T> logger) : base(engine, mainMenu, hotkeys)
        {
            _logger = logger;
        }

        public sealed override void ExecuteAction()
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
    }
}
