using NLog;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace KeyReceiverService.Services
{
    public class TcpServer : IDisposable
    {
        private bool _disposed;
        private readonly ILogger _logger;

        private readonly KeyEventProcessor _keyEventMessageProcessor;
        public TcpServer()
        {
            _keyEventMessageProcessor = new KeyEventProcessor(new KeyBoardProxy());
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task RunServer(CancellationToken stoppingToken)
        {
            var tcpListener = TcpListener.Create(8080); // todo get from config
            tcpListener.Start();
            var cancelTask = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(5000);
                }
            });

            while (!_disposed)
            {
                try
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    ProcessMessage(tcpClient);
                }
                catch (SocketException e)
                {
                    _logger.Error(e);
                }
            }
        }

        private async Task ProcessMessage(TcpClient tcp)
        {
            try
            {
                await using var stream = tcp.GetStream();
                await _keyEventMessageProcessor.ProcessAsync(stream);
                tcp.Dispose();
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public void Dispose() => _disposed = true;
    }
}
