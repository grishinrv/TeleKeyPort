using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KeyPusher.Configuration;

namespace KeyPusher.WinApi
{
    public class KeyEventsDetector : IDisposable
    {
        private const int WH_KEYBOARD_LL = 13; // enable hooking
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private readonly IntPtr _keyDown = (IntPtr) WM_KEYDOWN;
        private readonly IntPtr _keyUp = (IntPtr) WM_KEYUP;
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookId = IntPtr.Zero;
        private readonly HotKeysOptions _hotKeysOptions;

        public KeyEventsDetector(HotKeysOptions hotKeysOptions)
        {
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

        private bool Block() => BlockInput(true);
        private bool UnBlock() => BlockInput(false);
        public event Action<object, Models.KeyEventArgs> KeyEventHappened;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,GetModuleHandle(curModule.ModuleName), 0);
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var keyCode = Marshal.ReadInt32(lParam);
            if (nCode >= 0 )
            {
                if (wParam == _keyDown)
                    KeyEventHappened?.Invoke(this, new Models.KeyEventArgs((Keys)keyCode, WM_KEYDOWN));
                else if(wParam == _keyUp)
                    KeyEventHappened?.Invoke(this, new Models.KeyEventArgs((Keys)keyCode, WM_KEYUP));
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
