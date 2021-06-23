using KeyPusher.Menus;
using KeyPusher.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KeyPusher
{
    public class KeyPusherApp : ApplicationContext
    {
        private readonly KeyPusherEngine _engine;

        public KeyPusherApp(KeyPusherEngine engine, MenuPresenter menu, IEnumerable<IMenuItemPresenter> menuItems)
        {
            _engine = engine;
            menu.SetMenuItems(menuItems.ToList());
        }
    }
}