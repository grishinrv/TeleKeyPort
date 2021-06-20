using System;
using System.Collections.Generic;
using System.Linq;
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
            _trayIcon = new NotifyIcon()
            {
                //Icon = ToolTipIcon.Info,
                //Me
                //ContextMenu = new ContextMenu(new MenuItem[] {
                //    new MenuItem("Exit", Exit)
                //}),
                Visible = true
            };
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
