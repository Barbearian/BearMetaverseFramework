using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ItemPickUpNodeData : TriggerNodeData
    {

        private PickedUpSignal signal = new PickedUpSignal();
        public override void OnTriggerEnter(Collider collider)
        {
            Debug.Log("Someone entered");
            if (collider.TryGetComponent(out NodeView targetview) && 
                view.TryGetNodeData(out ItemPickerNodeData data)
                )
            {
                signal.node = targetview;
                view.ReceiveNodeSignal(signal);
            }
        }

        
    }

    public class PickedUpSignal : INodeSignal {
        public INode node;
    }

    
}