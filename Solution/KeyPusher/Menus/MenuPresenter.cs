using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace KeyPusher.Menus
{
    public class MenuPresenter : IDisposable
    {
        private readonly NotifyIcon _trayIcon;
        private IReadOnlyList<IMenuItemPresenter> _items;
        private readonly ContextMenuStrip _mainTrayMenu;
        public MenuPresenter(ContextMenuStrip mainTrayMenu)
        {
            _mainTrayMenu = mainTrayMenu;
            _mainTrayMenu.ItemClicked += OnTrayItemClicked;
            // load icon from resources
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("KeyPusher.Resources.gear.png");
            var bitmap = new Bitmap(stream);
            var pIcon = bitmap.GetHicon();
            var icon = Icon.FromHandle(pIcon);
            // set tray icon
            _trayIcon = new NotifyIcon
            {
                Icon = icon,
                ContextMenuStrip = mainTrayMenu,
                Visible = true
            };
            icon.Dispose();
        }

        public void Dispose()
        {
            _trayIcon.Visible = false;
            _mainTrayMenu.ItemClicked -= OnTrayItemClicked;
        }

        internal void SetMenuItems(IReadOnlyList<IMenuItemPresenter> menuItems)
        {
            _items = menuItems;
            foreach (var item in _items)
            {
                item.CreateView();
            }
        }

        private void OnTrayItemClicked(object sender, ToolStripItemClickedEventArgs e) => _items.First(x => x.ActionName == e.ClickedItem.Text).ExecuteAction();

        internal void OnStateChanged()
        {
            foreach (var item in _items)
            {
                item.StateChanged();
            }
        }

        public void InvokeHotkey(Keys key)
        {
            _items.FirstOrDefault(x => x.HotKeyCode != null && ((Keys)x.HotKeyCode) == key)
                ?.ExecuteAction();
        }
    }
}
