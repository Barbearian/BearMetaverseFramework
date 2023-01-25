using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class EquipmentManagerNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        public Dictionary<string,INode> equipments = new Dictionary<string,INode>();
        internal OnEquippedSignal OnEquippedSignal = new OnEquippedSignal();
        internal OnUnequippedSignal OnUnequippedSignal = new OnUnequippedSignal();


        public void Attached(INode node)
        {
            OnEquippedSignal.wielder= node;
            OnEquippedSignal.manager = this;

            OnUnequippedSignal.wielder = node;
            OnUnequippedSignal.manager= this;


            node.RegisterSignalReceiver<EquipSignal>((x) => {
                this.Equip(x.key,x.equipment);
            },true).AddTo(receivers);

            node.RegisterSignalReceiver<UnequipSignal>((x) => {
                this.Unequip(x.key);
            }, true).AddTo(receivers);
        }
    }

    public static class EquipmentManagerNodeDataSystem {
        public static void Equip(this EquipmentManagerNodeData data,string key,INode equipment) {
            var equipments = data.equipments;
            data.OnEquippedSignal.equipmentKey = key;


            if (equipments.TryGetValue(key, out var unquiped))
            {
                data.Unequip(key);
            }

            equipment.ReceiveNodeSignal(data.OnEquippedSignal);
        }

        public static void Unequip(this EquipmentManagerNodeData data, string key)
        {
            var equipments = data.equipments;
            if (equipments.TryGetValue(key, out var unquiped))
            {
                data.OnEquippedSignal.equipmentKey= key;
                unquiped.ReceiveNodeSignal(data.OnUnequippedSignal);
                equipments.Remove(key);
            }
        }
    }

    public class EquipSignal:INodeSignal {
        public INode equipment;
        public string key;
    }

    public class UnequipSignal : INodeSignal
    {
        public string key;
    }

    public enum eEquipmentSlot { 
        RightHand,
        LeftHand
    }
}