using System.Xml;

namespace Bear
{
    //An link between two nodes
    public class ItemToWielderNodeData : LinkNodeData{
        public override (string, string) GetKeys()
        {
            return GetKeys<OnEquippedSignal, OnUnequippedSignal>();
        }

        public override void OnInit(INode node)
        {
            base.OnInit(node);

            node.RegisterSignalReceiver<ItemActExSignal>((x) => {
                x.Receiver = Target;
                Target?.ReceiveNodeSignal<IExecutableNodeSignal>(x);
            },true).AddTo(receivers);
        }


    }

    
  
}