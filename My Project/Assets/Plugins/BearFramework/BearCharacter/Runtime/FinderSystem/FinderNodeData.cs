using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class FinderNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
    {
        private NodeView MyView;
        public Dictionary<string, Transform> Cache = new();

        public void Attached(INode node)
        {
            if (node is NodeView view) { 
                Init(view);
            }
        }

        public void Init(NodeView view) {
            MyView = view;
            Cache = new Dictionary<string, Transform>();
        }

        public void Detached(INode node)
        {
            
        }

        public bool TryFind(string name,out Transform view) {
            if (!Cache.TryGetValue(name, out view))
            {
                var bones = MyView.gameObject.GetComponentsInChildren<Transform>();
                foreach (Transform bone in bones)
                {
                    if (bone.CompareTag(name))
                    {
                        view = bone;
                        Cache[name] = view;
                        return true;
                    }
                }
            }
            else {
                return true;
            }

            return false;
        }
    }
}