using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using KeyPusher.Models;

namespace KeyPusher.Menus
{
    public class MenuController : IDisposable
    {
        private readonly ContextMenuStrip _mainTrayMenu;
        private readonly NotifyIcon _trayIcon;
        public MenuController(ContextMenuStrip mainTrayMenu)
        {
            _mainTrayMenu = mainTrayMenu;
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
                ContextMenuStrip = _mainTrayMenu,
                Visible = true
            };
            icon.Dispose();
        }

        private void OnTrayItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Exit")
                Exit();
        }

        public void Dispose()
        {
            _trayIcon.Visible = false;
            Application.Exit();
        }

        private Mode _appMode;

        public Mode AppMode
        {
            get => _appMode;
            set
            {
                if (_appMode != value)
                {
                    _appMode = value;
                    StateChanged?.Invoke(this);
                }
            }
        }

        public event Action<MenuController> StateChanged;
    }
}
