using KeyReceiverService.Configuration;
using KeyReceiverService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

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
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                        loggingBuilder.AddNLog(configuration);
                    });
                });
    }
}
