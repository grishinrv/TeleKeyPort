using KeyPusher.Configuration;
using KeyPusher.Services;
using KeyPusher.WinApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Configuration;
using System;
using System.Windows.Forms;
using KeyPusher.Menus;

namespace KeyPusher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var host = CreateHostBuilder().Build();
            var services = host.Services;
            var application = services.GetRequiredService<KeyPusherApp>();
            application.Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    services.AddSingleton(configuration.GetSection("WebConfig").Get<ConnectionOptions>())
                        .AddSingleton(configuration.GetSection("HotKeys").Get<HotKeysOptions>())
                        .AddSingleton<KeyPusherApp>()
                        .AddSingleton<KeyEventsDetector>()
                        .AddSingleton<MenuPresenter>()
                        .AddSingleton<ContextMenuStrip>()
                        .AddSingleton<KeyPusherEngine>()
                        .AddTransient<TcpChannel>()
                        .AddTransient<IMenuItemPresenter, ExitMenuItem>()
                        .AddTransient<IMenuItemPresenter, EnableHookMenuItem>()
                        .AddTransient<IMenuItemPresenter, DisableHookMenuItem>();
                })
                .ConfigureLogging((hostBuilderContext, logging) =>
                {
                    logging.AddFileLogger(options =>
                    {
                        hostBuilderContext.Configuration.GetSection("Logging").GetSection("FileLogger").GetSection("Options").Bind(options);
                    });
                });
    }
}
