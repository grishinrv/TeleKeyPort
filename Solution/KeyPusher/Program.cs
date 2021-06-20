using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyPusher.WinApi;

namespace KeyPusher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KeyPusherApp());
        }
    }

    public class KeyPusherApp : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private KeyEventsDetector _keysDetector;

        public KeyPusherApp()
        {
            _keysDetector = new KeyEventsDetector();
            _keysDetector.KeyEventHappened += (o, args) => MessageBox.Show($"{args.Key}");
            // load icon from resources
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("KeyPusher.Resources.gear.png");
            var bitmap = new Bitmap(stream);
            var pIcon = bitmap.GetHicon();
            var icon = Icon.FromHandle(pIcon);
            // set tray icon
            _trayIcon = new NotifyIcon()
            {
                Icon = icon,
                //Me
                //ContextMenu = new ContextMenu(new MenuItem[] {
                //    new MenuItem("Exit", Exit)
                //}),
                Visible = true
            };
            icon.Dispose();
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            _keysDetector.Dispose();
            Application.Exit();
        }
    }
}
