using KeyReceiverService.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace KeyReceiverService.Services
{
    public class TcpServer : IDisposable
    {
        private bool _disposed;
        private readonly int _port;
        private readonly string _ipTemplate;

        private readonly ILogger<TcpServer> _logger;
        private readonly KeyEventProcessor _keyEventMessageProcessor;

        public TcpServer(KeyEventProcessor keyEventProcessor, WorkerOptions options, ILogger<TcpServer> logger)
        {
            _keyEventMessageProcessor = keyEventProcessor;
            _logger = logger;
            _port = options.Port;
            _ipTemplate = options.IpTemplate;
        }

        public async Task RunServer(CancellationToken stoppingToken)
        {
            var tcpListener = TcpListener.Create(_port);
            tcpListener.Start();

            while (!_disposed && !stoppingToken.IsCancellationRequested)
            {
                    var tcpClient = await ExecuteAsync(tcpListener.AcceptTcpClientAsync(), 5000);
                    if (ValidateClient(tcpClient))
#pragma warning disable 4014
                        ProcessMessage(tcpClient);
#pragma warning restore 4014
            }
        }

        private bool ValidateClient(TcpClient tcp)
        {
            bool result = false;
            if (tcp != null)
            {
                var ip = ((IPEndPoint) tcp.Client.RemoteEndPoint).Address.ToString();
                _logger.LogInformation("Attempt to connect from ip {0}...", ip);
                result = ip.Contains(_ipTemplate);
                _logger.LogInformation("Attempt to connect from ip {0}... Success - {1}", ip, result);
            }
            return result;
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
                _logger.LogError(e, "Error when trying to receive tcp socket connection");
            }
            return default(T);
        }

        private async Task ProcessMessage(TcpClient tcp)
        {
            var ip = ((IPEndPoint)tcp.Client.RemoteEndPoint).Address.ToString();
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
                _logger.LogError(e, "Error when trying to process data from socket");
            }
            _logger.LogInformation("Disconnect client from ip {0}...", ip);
        }

        public void Dispose() => _disposed = true;
    }
}
