using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class Testor : NodeView
	{
	    // Start is called before the first frame update
	    void Start()
	    {
		    this.GetOrCreateNodeData(new InputAssociateNodeData());
	    }
	

	}
}