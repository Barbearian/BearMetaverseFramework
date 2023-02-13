
namespace Bear
{
    public class SignalHandlerNodeData : INodeData, IOnDetachedFromNode
    {
        protected ArrayNodeSignalReceiver receivers = new ArrayNodeSignalReceiver();


        public virtual void Detached(INode node)
        {
            receivers.InhibitAll();
        }

       
    }
}