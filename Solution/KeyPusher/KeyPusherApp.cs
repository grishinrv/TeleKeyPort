using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using KeyPusher.Configuration;
using KeyPusher.Services;
using KeyPusher.WinApi;

namespace KeyPusher
{
    public class KeyPusherApp : ApplicationContext
    {
        private readonly KeyEventsDetector _keysDetector;
        private readonly TcpChannel _tcp;

        public KeyPusherApp(TcpChannel tcp, KeyEventsDetector keysDetector)
        {
            _tcp = tcp;
            _keysDetector = keysDetector;
            _keysDetector.KeyEventHappened += (o, args) => MessageBox.Show($"{args.Key}");
        }

        

        private void Exit()
        {
            _keysDetector.Dispose();
            _tcp.Dispose();
        }
    }
}