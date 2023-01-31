using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ItemManagerNodeData : LinkNodeData{


        public override (string, string) GetKeys()
        {
            return (typeof(EquipSignal).ToString(), typeof(UnequipSignal).ToString());
        }
        public override void Delink(INode node)
        {
          

            base.Delink(node);
            node.ReceiveNodeSignal(new OnUnequippedSignal()
            {
                wielder = Source,
                manager = this
            });
        }

        public override void Link(INode node)
        {
            base.Link(node);
            node.ReceiveNodeSignal(new OnEquippedSignal()
            {
                wielder = Source,
                manager = this
            });
        }
    }

    public struct EquipSignal:INodeSignal,ILinkSignal {
        public INode equipment;
        public INode Target { get => equipment; set => equipment = value; }
    }

    public struct UnequipSignal : INodeSignal,IDelinkSignal
    {
        public INode equipment;
        public INode Target { get => equipment; set => equipment = value; }
    }

}