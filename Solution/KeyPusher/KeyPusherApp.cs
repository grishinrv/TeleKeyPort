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
        private readonly KeyPusherEngine _engine;
        private readonly ILogger<KeyPusherApp> _logger;

        public KeyPusherApp(ILogger<KeyPusherApp> logger, KeyPusherEngine engine, MenuPresenter menu, IEnumerable<IMenuItemPresenter> menuItems)
        {
            _logger = logger;
            _engine = engine;
            menu.SetMenuItems(menuItems.ToList());
        }

        public void Run()
        {
            _logger.LogInformation("Starting application...");
            Application.Run(this);
            _logger.LogInformation("Application started.");
        }

        public void ShutDown()
        {

        }
    }
}