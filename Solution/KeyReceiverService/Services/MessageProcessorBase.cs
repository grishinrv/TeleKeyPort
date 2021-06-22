using MessagePack;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KeyReceiverService.Services
{
    public abstract class MessageProcessorBase
    {
        private async Task<byte[]> ReadFromStreamAsync(NetworkStream stream, int capacity)
        {
            var buf = new byte[capacity];
            var currentPosition = 0;
            while (currentPosition < capacity)
                currentPosition += await stream.ReadAsync(buf, currentPosition, capacity - currentPosition);
            return buf;
        }

        internal async Task ProcessAsync(NetworkStream stream) => DeserializeAndProcess(await ReadFromStreamAsync(stream, 2));

        protected abstract void DeserializeAndProcess(byte[] buffer);
    }

    public abstract class MessageProcessorBase<T> : MessageProcessorBase
    {
        protected sealed override void DeserializeAndProcess(byte[] buffer)
        {
            var message = MessagePackSerializer.Deserialize<T>(buffer);
            Process(message);
        }

        protected abstract void Process(T message);
    }
}
