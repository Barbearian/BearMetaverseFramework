
namespace Bear
{
    public class EquipmentOnPickUpNodeData : INodeData, IOnAttachedToNode,IOnDetachedFromNode
    {
        public INode equipment;
        private ArrayNodeSignalReceiver receivers { get; set; } = new ArrayNodeSignalReceiver();
        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<PickedUpSignal>((x) => {
                x.node.ReceiveNodeSignal(new PickUpEquipmentSignal() { 
                    Equipment = equipment,
                });
                equipment = null;


            },true).AddTo(receivers);
        }

        public void Detached(INode node)
        {
            receivers.InhibitAll();
            if(equipment != null) equipment.Dispose();
        }

        public EquipmentOnPickUpNodeData(INode data)
        {
            this.equipment = data;

        }




    }

    public class PickUpEquipmentSignal : INodeSignal {
        public INode Equipment;
    }
}