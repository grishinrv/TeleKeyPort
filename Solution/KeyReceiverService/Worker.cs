using KeyReceiverService.Services;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KeyReceiverService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly TcpServer _server;

        public Worker()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _server = new TcpServer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _server.RunServer(stoppingToken);
            _server.Dispose();
        }
    }
}