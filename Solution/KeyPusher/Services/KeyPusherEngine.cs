﻿using KeyPusher.Menus;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyPusher.WinApi;
using MessagePack;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure;
using Shared.Models;

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
                    _keysDetector.InputBlocked = _enabled;
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
#if DEBUG
            _logger.LogDebug("Key code: {0}, event code: {1}", eventArgs.Key, eventArgs.EventCode);
#endif
            if (eventArgs.EventCode == KeyCodes.WM_KEYDOWN)
                _menu.InvokeHotkey(eventArgs.Key);

            if (Enabled)
            {
                var dto = new KeyEventMessage {EventCode = (uint)eventArgs.EventCode, KeyCode = (byte)eventArgs.Key};
                var bytes = MessagePack.MessagePackSerializer.Serialize(dto, MessagePackSerializerOptions.Standard);
                Task.Run(async () => await _tcp.Send(bytes));
            }
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
