using KeyPusher.Menus;
using KeyPusher.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace KeyPusher
{
    public class KeyPusherApp : ApplicationContext
    {
        private readonly ILogger<KeyPusherApp> _logger;
        private readonly MenuPresenter _mainMenu;
        private readonly IReadOnlyList<IMenuItemPresenter> _menuItems;

        public KeyPusherApp(ILogger<KeyPusherApp> logger, MenuPresenter menu, IEnumerable<IMenuItemPresenter> menuItems)
        {
            _logger = logger;
            _mainMenu = menu;
            _menuItems = menuItems.ToList();
        }

        public void Run()
        {
            _logger.LogInformation("Starting application...");
            _mainMenu.SetMenuItems(_menuItems);
            _logger.LogInformation("Application started.");
            Application.Run(this);
        }

        public void ShutDown()
        {

        }
    }
}