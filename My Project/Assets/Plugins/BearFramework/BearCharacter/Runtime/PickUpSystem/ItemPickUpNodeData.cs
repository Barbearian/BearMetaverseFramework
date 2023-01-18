using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ItemPickUpNodeData : TriggerNodeData
    {
        
        public override void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out NodeView view) && 
                view.TryGetNodeData(out ItemPickerNodeData data)
                )
            {
                data.PickUp(this.GetNodeDataRoot());        
            }
        }
    }

    
}