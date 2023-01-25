using UnityEngine;

namespace Bear
{
    public class EquipmentNodeData : SignalHandlerNodeData,IOnAttachedToNode
    {
        public INode Wielder { get; set; }
        public EquipmentData data;

        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<OnEquippedSignal>((x) => {
                Wielder = x.wielder;
            },true).AddTo(receivers);
        }


        
    }

    public static class EquipmentNodeDataSystem {
        public static void Equip(this INode Wielder,INode weapon) {
            Debug.Log(Wielder+" Equiped "+ weapon);
        }
    }

    public struct OnEquippedSignal : INodeSignal {
        public string equipmentKey;
        public INode wielder;
        public EquipmentManagerNodeData manager;
    }

    public struct OnUnequippedSignal : INodeSignal
    {
        public string equipmentKey;
        public INode wielder;
        public EquipmentManagerNodeData manager;
    }
}