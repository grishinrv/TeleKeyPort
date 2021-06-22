using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace KeyReceiverService.Services
{
    public class TcpServer : IDisposable
    {
        private bool _disposed;

        private readonly KeyEventProcessor _keyEventMessageProcessor;
        public TcpServer()
        {
            _keyEventMessageProcessor = new KeyEventProcessor(new KeyBoardProxy());
        }

        public async void RunServer()
        {
            var tcpListener = TcpListener.Create(8080); // todo get from config
            tcpListener.Start();
            while (!_disposed)
            {
                using var tcpClient = await tcpListener.AcceptTcpClientAsync();
#pragma warning disable 4014
                ProcessMessage(tcpClient);
#pragma warning restore 4014
            }
        }

        private async Task ProcessMessage(TcpClient tcp)
        {
            await using var stream = tcp.GetStream();
            await _keyEventMessageProcessor.ProcessAsync(stream);
        }

        public void Dispose() => _disposed = true;
    }
}
