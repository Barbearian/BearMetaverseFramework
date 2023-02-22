using System.Collections.Generic;

namespace Bear
{

    public class ItemStateMachineNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        private ItemNodeData item;
        private DynamicStateMachineNodeData sm;
        private INode root;
        public void Attached(INode node)
        {
            root = node;
            sm = node.GetOrCreateNodeData<DynamicStateMachineNodeData>();
            item = node.GetOrCreateNodeData<ItemNodeData>();
            node.RegisterSignalReceiver<ItemStateMachineSignal>((x) => {
                ProcessSignal(x);
            },true).AddTo(receivers);

            node.RegisterSignalReceiver<ItemStateEnterSignal>((x) => {
                UpdateState(x.index);
            },true).AddTo(receivers);

            node.RegisterSignalReceiver<ItemEnterDefaultStateSignal>((x) => {
                sm.EnterDefault();
            }, true).AddTo(receivers);
        }

        public void ProcessSignal(ItemStateMachineSignal signal)
        {
            if (sm.TryPredict(signal.signal) && 
                sm.TryGetPredictedSignal(out var prediction) &&
                prediction is SignalContainerTransferSignal sctsignal) {

                root.ReceiveNodeSignal(sctsignal);
            }
        }

        public INodeSignal Translate(ItemStateMachineSignal signal) {
            var rs = signal.signal;
            return rs;
        }

        public void UpdateState(int index) {
            sm.EnterState(index);
        }

    }

    public class ItemActExSignal : ISignalSenderExSignal, IItemActSignal
    {
        public INode Receiver { get; set; }
        public INodeSignal[] signals { get; set; } 

        public void Execute()
        {
            foreach (var signal in signals)
            {
                Receiver.ReceiveNodeSignal(signal);
            }
        }
    }
    public struct ItemActFilter : ISignalFilter
    {
        public bool CanPass(INodeSignal signal = null)
        {
            return signal is not ItemActExSignal;
        }


        public override bool Equals(object obj)
        {
            return GetType().Equals(obj.GetType());
        }

        public override int GetHashCode()
        {

            return GetType().GetHashCode();
        }
    }
    public struct ItemStateMachineSignal : INodeSignal {
        public INodeSignal signal;
    }

    public interface IItemActSignal { }

    public struct ItemStateEnterSignal:INodeSignal {
        public int index;
    }

    public struct ItemEnterDefaultStateSignal : INodeSignal { 
    
    }
   

    

}