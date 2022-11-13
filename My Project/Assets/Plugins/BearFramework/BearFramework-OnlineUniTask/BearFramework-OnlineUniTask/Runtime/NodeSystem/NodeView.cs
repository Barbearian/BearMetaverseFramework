using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    public class NodeView : MonoBehaviour, INode
    {
        
	    private void OnDestroy() {
		    Debug.Log("I am destoryed");
            this.Dispose();    
	    }

    }
}