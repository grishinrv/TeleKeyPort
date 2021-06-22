using NLog;
using System;
using System.Configuration;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace KeyReceiverService.Services
{
    public class TcpServer : IDisposable
    {
        private bool _disposed;
        private readonly ILogger _logger;
        private readonly int _port;

        private readonly KeyEventProcessor _keyEventMessageProcessor;
        public TcpServer(int port)
        {
            _keyEventMessageProcessor = new KeyEventProcessor(new KeyBoardProxy());
            _logger = LogManager.GetCurrentClassLogger();
            _port = port;
        }

        public async Task RunServer(CancellationToken stoppingToken)
        {
            var tcpListener = TcpListener.Create(_port);
            tcpListener.Start();

            while (!_disposed && stoppingToken.IsCancellationRequested)
            {
                    var tcpClient = await ExecuteAsync(tcpListener.AcceptTcpClientAsync(), 5000);
                    if (tcpClient != null)
#pragma warning disable 4014
                        ProcessMessage(tcpClient);
#pragma warning restore 4014
            }
        }

        private async Task<T> ExecuteAsync<T>(Task<T> task, int millisecondsTimeout)
        {
            try
            {
                if (await Task.WhenAny(task, Task.Delay(millisecondsTimeout)) == task)
                    return task.Result;
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            return default(T);
        }

        private async Task ProcessMessage(TcpClient tcp)
        {
            try
            {
                using (tcp)
                {
                    await using var stream = tcp.GetStream();
                    await _keyEventMessageProcessor.ProcessAsync(stream);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public void Dispose() => _disposed = true;
    }
}
