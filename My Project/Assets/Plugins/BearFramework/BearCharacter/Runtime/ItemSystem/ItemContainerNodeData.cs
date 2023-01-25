using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ItemContainerNodeData : INodeData, IOnAttachedToNode, IOnDetachedFromNode
    {
        public int Max;
        public List<ItemData> data= new List<ItemData>();

        public void Attached(INode node)
        {
        }

        public void Detached(INode node)
        {
        }
    }


}