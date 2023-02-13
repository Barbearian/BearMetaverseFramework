using System.Collections.Generic;

namespace Bear
{
    public class SignalContainerNodeData : SignalHandlerNodeData,IOnAttachedToNode
    {
        private Dictionary<string, INodeSignal> container = new Dictionary<string, INodeSignal>();

        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<SignalContainerTransferSignal>((x) => {
                if (container.TryGetValue(x.key,out var signal)) {
                    node.ReceiveNodeSignal(signal);
                }
            },true).AddTo(receivers);

        }

        public void Register(SignalContainerTransferSignal key,INodeSignal value) {
            container[key.key] = value;
        }
    }

    public struct SignalContainerTransferSignal:INodeSignal {
        public string key;
    }

}