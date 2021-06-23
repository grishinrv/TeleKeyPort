using KeyReceiverService.Services;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using KeyReceiverService.Configuration;
using Microsoft.Extensions.Logging;

namespace KeyReceiverService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly TcpServer _server;

        public Worker(ILogger<Worker>  logger, TcpServer server)
        {
            _logger = logger;
            _server = server;
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service started");
            using (_server)
                await _server.RunServer(stoppingToken);
            _logger.LogInformation("Service stopped");
        }
    }
}
