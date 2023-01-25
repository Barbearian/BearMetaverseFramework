using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ItemPickUpNodeData : TriggerNodeData
    {
        private PickUpSignal signal = new PickUpSignal();
        private PickedUpSignal pickedUp = new PickedUpSignal();
        public override void Attached(INode node)
        {
            base.Attached(node);
            signal.item = view;
            
        }

        public override void OnTriggerEnter(Collider collider)
        {
            if (
                collider.TryGetComponent(out NodeView targetview) &&
                targetview.TryGetNodeData(out ItemPickerNodeData data)
                )
            {
                pickedUp.picker = targetview;
                view.ReceiveNodeSignal(pickedUp);
            }
        }

        
    }

    public class PickedUpSignal : INodeSignal {
        public INode picker;
    }

    public class PickUpSignal : INodeSignal {
        public INode item;
    }
    
}