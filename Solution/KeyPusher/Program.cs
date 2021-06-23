using KeyPusher.WinApi;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using KeyPusher.Configuration;
using Microsoft.Extensions.Configuration;
using Shared.Infrastructure;

namespace KeyPusher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Utils.GetApplicationRootPath())
                .AddJsonFile("appsettings.json", true, false);
            var config = builder.Build();
            var connectionOptions = config.GetSection("WebConfig").Get<ConnectionOptions>();
            var hotKeysOptions = config.GetSection("HotKeys").Get<HotKeysOptions>();
            Application.Run(new KeyPusherApp(connectionOptions, hotKeysOptions));
        }
    }

    public class KeyPusherApp : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private KeyEventsDetector _keysDetector;

        public KeyPusherApp(ConnectionOptions connectionOptions, HotKeysOptions hotKeysOptions)
        {
            _keysDetector = new KeyEventsDetector(hotKeysOptions);
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
