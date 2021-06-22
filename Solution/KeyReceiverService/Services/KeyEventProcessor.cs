using Shared;

namespace KeyReceiverService.Services
{
    public class KeyEventProcessor : MessageProcessorBase<KeyEventMessage>
    {
        private readonly KeyBoardProxy _proxy;
        public KeyEventProcessor(KeyBoardProxy proxy)
        {
            _proxy = proxy;
        }

        protected sealed override void Process(KeyEventMessage message)
        {
            //todo
        }
    }
}
