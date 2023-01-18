using UnityEngine;

namespace Bear
{
    public class EquipmentNodeData : INodeData,IOnAttachedToNode
    {
        public NodeView Wielder { get; set; }
        private OnEquipSignal _signal = new OnEquipSignal();
        public OnEquipSignal Signal { get { 
                _signal.eData = this;
                return _signal; } }
        public void Attached(INode node)
        {
            
        }


        
    }

    public static class EquipmentNodeDataSystem {
        public static void Equip(this INode Wielder,EquipmentNodeData data) {
            if (Wielder is NodeView view) { 
                data.Wielder= view;
                data.GetNodeDataRoot().ReceiveNodeSignal(data.Signal);
            }
        }
    }

    public class OnEquipSignal : INodeSignal {
        public EquipmentNodeData eData;
    }
}