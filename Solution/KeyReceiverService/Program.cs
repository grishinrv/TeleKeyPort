using KeyReceiverService.Configuration;
using KeyReceiverService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KeyReceiverService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var options = configuration.GetSection("WebConfig").Get<WorkerOptions>();
                    services.AddSingleton(options);
                    services.AddSingleton<KeyBoardProxy>();
                    services.AddSingleton<TcpServer>();
                    services.AddTransient<KeyEventProcessor>();
                    services.AddHostedService<Worker>();
                });
    }
}
