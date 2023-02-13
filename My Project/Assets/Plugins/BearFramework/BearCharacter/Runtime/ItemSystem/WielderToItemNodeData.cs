namespace Bear
{
    public class WielderToItemNodeData : LinkNodeData
    {
        public override (string, string) GetKeys()
        {
            return GetKeys<EquipSignal, UnequipSignal>();
        }

        public override void OnInit(INode node)
        {
            base.OnInit(node);

            node.RegisterSignalReceiver<WielderToItemSignal>((x) => {
                Target?.ReceiveNodeSignal(x.signal);
            }, true).AddTo(receivers);
        }
    }

    public interface IWielderToItemSignal : INodeSignal 
    {
        public INodeSignal signal { get; set; }
    }

    public struct WielderToItemSignal : IWielderToItemSignal
    {
        public INodeSignal signal { get ; set ; }
    }



}