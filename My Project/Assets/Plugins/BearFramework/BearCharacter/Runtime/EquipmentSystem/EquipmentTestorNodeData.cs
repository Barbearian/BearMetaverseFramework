namespace Bear
{
    public class EquipmentTestorNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        public EquipSignal signal;
        
        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<PickedUpSignal>((x) => {
                x.picker.ReceiveNodeSignal(signal);
            },true).AddTo(receivers);
        }
    }
}