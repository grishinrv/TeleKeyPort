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

            while (!_disposed)
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
