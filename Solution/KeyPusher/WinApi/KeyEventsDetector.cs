using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KeyPusher.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure;

namespace KeyPusher.WinApi
{
    public class KeyEventsDetector : IDisposable
    {
        private readonly IntPtr _keyDown = (IntPtr) KeyCodes.WM_KEYDOWN;
        private readonly IntPtr _keyUp = (IntPtr) KeyCodes.WM_KEYUP;
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookId = IntPtr.Zero;
        private readonly HotKeysOptions _hotKeysOptions;
        private readonly ILogger<KeyEventsDetector> _logger;

        public KeyEventsDetector(ILogger<KeyEventsDetector> logger, HotKeysOptions hotKeysOptions)
        {
            _logger = logger;
            _hotKeysOptions = hotKeysOptions;
            _proc = HookCallback;
            _hookId = SetHook(_proc);
        }
        public void Dispose()
        {
            UnhookWindowsHookEx(_hookId);
            UnBlock();
        }

        private bool _inputBlocked;
        public bool InputBlocked
        {
            get => _inputBlocked;
            set
            {
                if (_inputBlocked != value)
                {
                    _inputBlocked = value;
                    if (_inputBlocked)
                        Block();
                    else
                        UnBlock();
                }
            }
        }

        private bool Block()
        {
            var result = BlockInput(true);
            _logger.LogInformation("Block input, success - {0}", result);
            return result;
        }

        private bool UnBlock()
        {
            var result = BlockInput(false);
            _logger.LogInformation("Unlock input, success - {0}", result);
            return result;
        }

        public event Action<object, Models.KeyEventArgs> KeyEventHappened;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            return SetWindowsHookEx(KeyCodes.WH_KEYBOARD_LL, proc,GetModuleHandle(curModule.ModuleName), 0);
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var keyCode = Marshal.ReadInt32(lParam);
            if (nCode >= 0 )
            {
                if (wParam == _keyDown)
                    KeyEventHappened?.Invoke(this, new Models.KeyEventArgs((Keys)keyCode, KeyCodes.WM_KEYDOWN));
                else if(wParam == _keyUp)
                    KeyEventHappened?.Invoke(this, new Models.KeyEventArgs((Keys)keyCode, KeyCodes.WM_KEYUP));
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        #region Unmanaged interop
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BlockInput(bool fBlockIt);
        #endregion
    }
}
