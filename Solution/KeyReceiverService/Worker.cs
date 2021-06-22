using KeyReceiverService.Services;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using KeyReceiverService.Configuration;

namespace KeyReceiverService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly TcpServer _server;

        public Worker(TcpServer server)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _server = server;
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Serive started");
            using (_server)
                await _server.RunServer(stoppingToken);
            _logger.Info("Serive stopped");
        }
    }
}
