using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KeyReceiverService.Services;

namespace KeyReceiverService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly KeyBoardProxy _keyBoard;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _keyBoard = new KeyBoardProxy();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _keyBoard.PressKey();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
