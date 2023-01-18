using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class PickUpViewNodeData : INodeData, IOnAttachedToNode
    {
        public IFetcher<GameObject> view;

        public PickUpViewNodeData(IFetcher<GameObject> view) {
            this.view = view;
        }
        public void Attached(INode node)
        {
            if (node is NodeView nview) {
                var obj = view.GetValue();
                nview.transform.PlaceAtChild(obj.transform);
            }

        }
    }

    public static class PlaceSystem {
        public static void PlaceAtChild(this Transform parent, Transform kid) {
            if (parent != null && kid != null)
            {

                kid.parent = parent;
                kid.transform.localPosition = Vector3.zero;
                kid.transform.localRotation = Quaternion.identity;
            }
        }
    }
}