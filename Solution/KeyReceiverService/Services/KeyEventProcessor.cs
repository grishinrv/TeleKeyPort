using Microsoft.Extensions.Logging;
using Shared;

namespace KeyReceiverService.Services
{
    public class KeyEventProcessor : MessageProcessorBase<KeyEventMessage>
    {
        private readonly KeyBoardProxy _proxy;
        private readonly ILogger<KeyBoardProxy> _logger;
        public KeyEventProcessor(KeyBoardProxy proxy, ILogger<KeyBoardProxy> logger)
        {
            _proxy = proxy;
            _logger = logger;
        }

        protected sealed override void Process(KeyEventMessage message)
        {
            //todo
        }
    }
}
