using KeyPusher.Menus;
using System;
using System.Windows.Forms;
using KeyPusher.WinApi;

namespace KeyPusher.Services
{
    public class KeyPusherEngine : IDisposable
    {
        private bool _enabled;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    _menu.OnStateChanged();
                }
            }
        }

        private readonly MenuPresenter _menu;
        private readonly KeyEventsDetector _keysDetector;
        private readonly TcpChannel _tcp;
        public KeyPusherEngine(MenuPresenter menu, TcpChannel tcp, KeyEventsDetector keysDetector)
        {
            _menu = menu;
            _tcp = tcp;
            _keysDetector = keysDetector;
            //_keysDetector.KeyEventHappened += (o, args) => MessageBox.Show($"{args.Key}");
        }

        public void Dispose()
        {
            _menu.Dispose();
            _keysDetector.Dispose();
            _tcp.Dispose();
            Application.Exit();
        }
    }
}
