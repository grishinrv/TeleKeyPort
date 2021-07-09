using KeyPusher.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KeyPusher.Services
{
    public class TcpChannel : IDisposable
    {
        private readonly TcpClient _tcp;
        private readonly ConnectionOptions _options;
        private readonly ILogger<TcpChannel> _logger;
        public TcpChannel(ConnectionOptions options, ILogger<TcpChannel> logger)
        {
            _logger = logger;
            _options = options;
            _tcp = new TcpClient();
        }

        public async Task Send(byte[] buffer, int offset = 0)
        {
            try
            {
                await InsureConnected();
                await using var stream = _tcp.GetStream();
                await stream.WriteAsync(buffer, offset, buffer.Length);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sending message error");
            }
        }

        private async ValueTask InsureConnected()
        {
            if (!_tcp.Connected)
            {
                _logger.LogInformation("Begin connection...");
                try
                {
                    await _tcp.ConnectAsync(IPAddress.Parse(_options.ReceiverIp), _options.ReceiverPort);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                    throw e;
                }
                _logger.LogInformation("Begin connection... Success");
            }
        }

        public void Dispose() => _tcp.Dispose();
    }
}
