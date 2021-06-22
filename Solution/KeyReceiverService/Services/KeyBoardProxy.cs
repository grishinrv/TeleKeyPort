using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace KeyReceiverService.Services
{
    public class KeyBoardProxy
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public void PressKey()
        {
            const int VK_UP = 0x26; //up key
            const int VK_DOWN = 0x28; //down key
            const int VK_LEFT = 0x25;
            const int VK_RIGHT = 0x27;
            const uint KEYEVENTF_KEYUP = 0x0002;
            const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
            const int WM_KEYDOWN = 0x0100;

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    press();
                }


                int press()
                {
                    //Press the key
                    keybd_event((byte) 0x45, 0, WM_KEYDOWN | 0, 0);
                    return 0;
                }

                int release()
                {
                    //Release the key
                    keybd_event((byte) VK_RIGHT, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                    return 0;
                }
            });
        }
    }
}
