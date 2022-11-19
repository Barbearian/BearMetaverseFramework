using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    public class NodeView : MonoBehaviour, INode
    {
        ANode node =  new ANode();
	    private void OnDestroy() {
		    Debug.Log("I am destoryed");
            this.Dispose();    
	    }

        public override int GetHashCode()
        {
            return node.GetHashCode();
        }

        public INode GetNode()
        {
            return node;
        }

        public override bool Equals(object other)
        {
            if (other is INode view)
            {
                return node.Equals(view.GetNode());

            }
            return false;
        }
        
	    public virtual void Awake(){
		    var parent = transform.parent;
		    if(parent != null && parent.TryGetComponent<NodeView>(out var view)){
		    	view.AddChildrenNode(this);
		    }
	    }

    }
}