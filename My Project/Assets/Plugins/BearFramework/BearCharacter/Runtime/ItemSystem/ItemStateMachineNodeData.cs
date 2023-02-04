namespace Bear
{

    public class ItemStateMachineNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        private ItemNodeData item;
        public void Attached(INode node)
        {
            item = node.GetOrCreateNodeData<ItemNodeData>();
            node.RegisterSignalReceiver<ItemStateMachineSignal>((x) => { 
                item.Wielder.ReceiveNodeSignal<IExecutableNodeSignal>(Translate(x));
            },true).AddTo(receivers);
        }

        public INodeSignal Translate(ItemStateMachineSignal signal) {
            var rs = signal.signal;
            if (rs is ItemActExSignal irs) { 
                irs.receiver = item.Wielder;
            }
            return rs;
        }


    }

    public class ItemActExSignal : SignalSenderExSignal, IItemActSignal{}
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
    

   

    

}