using KeyPusher.Menus;
using System;
using System.Windows.Forms;
using KeyPusher.WinApi;
using Microsoft.Extensions.Logging;

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
                    _keysDetector.EnableHooking = _enabled;
                }
            }
        }

        private readonly MenuPresenter _menu;
        private readonly KeyEventsDetector _keysDetector;
        private readonly TcpChannel _tcp;
        private readonly ILogger<KeyPusherEngine> _logger;
        public KeyPusherEngine(ILogger<KeyPusherEngine> logger, MenuPresenter menu, TcpChannel tcp, KeyEventsDetector keysDetector)
        {
            _logger = logger;
            _menu = menu;
            _tcp = tcp;
            _keysDetector = keysDetector;
            _keysDetector.KeyEventHappened += OnKeyEvent;
        }

        private void OnKeyEvent(object source, Models.KeyEventArgs eventArgs)
        {
            _logger.LogInformation("Key code: {0}, event code: {1}", eventArgs.Key, eventArgs.EventCode);
        }

        public void Dispose()
        {
            _keysDetector.KeyEventHappened -= OnKeyEvent;
            _menu.Dispose();
            _keysDetector.Dispose();
            _tcp.Dispose();
            Application.Exit();
        }
    }
}
