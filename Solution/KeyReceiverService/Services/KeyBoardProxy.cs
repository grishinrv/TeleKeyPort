using System.Runtime.InteropServices;

namespace KeyReceiverService.Services
{
    public class KeyBoardProxy
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public void Execute(byte keyCode, uint eventCode)
        {
            keybd_event(keyCode, 0, eventCode | 0, 0);
        }
    }
}
