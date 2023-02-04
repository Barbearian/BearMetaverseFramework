using UnityEngine;

namespace Bear
{
    public class ItemNodeData : SignalHandlerNodeData,IOnAttachedToNode
    {
        public INode Wielder { get; set; }

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

    public struct OnEquippedSignal : INodeSignal, ILinkSignal {
        public INode wielder;
        public ItemManagerNodeData manager;

        public INode Target { get => wielder; set => wielder = value; }
    }

    public struct OnUnequippedSignal : INodeSignal, IDelinkSignal
    {
        public INode wielder;
        public ItemManagerNodeData manager;

        public INode Target { get => wielder; set => wielder = value; }

    }
}