

namespace Bear
{
    public class ItemPickerNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<PickUpSignal>((x) => { 
                

            }).AddTo(receivers);
        }
    }
}