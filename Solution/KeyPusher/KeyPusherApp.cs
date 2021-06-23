using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using KeyPusher.Configuration;
using KeyPusher.WinApi;

namespace KeyPusher
{
    public class KeyPusherApp : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private KeyEventsDetector _keysDetector;

        public KeyPusherApp(ConnectionOptions connectionOptions, KeyEventsDetector keysDetector)
        {
            _keysDetector = keysDetector;
            _keysDetector.KeyEventHappened += (o, args) => MessageBox.Show($"{args.Key}");
            // load icon from resources
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("KeyPusher.Resources.gear.png");
            var bitmap = new Bitmap(stream);
            var pIcon = bitmap.GetHicon();
            var icon = Icon.FromHandle(pIcon);

            // set tray icon
            var trayContextMenu = new ContextMenuStrip();
            trayContextMenu.Items.Add("Exit");
            trayContextMenu.ItemClicked += OnTrayItemClicked;
            _trayIcon = new NotifyIcon
            {
                Icon = icon,
                ContextMenuStrip = trayContextMenu,
                Visible = true
            };
            icon.Dispose();
        }

        private void OnTrayItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Exit")
                Exit();
        }

        private void Exit()
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            _keysDetector.Dispose();
            Application.Exit();
        }
    }
}