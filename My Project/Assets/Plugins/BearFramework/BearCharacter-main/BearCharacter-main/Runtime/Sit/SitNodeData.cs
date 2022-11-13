using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear{
	public class SitNodeData : INodeData
	{
		public IAnchorNodeData anchor;
		public bool CanSit;
	}
	
	public static class SitNodeDataSystem{
		public static void SitOn(this NodeView view, SitNodeData sitNodeData){
			if(sitNodeData.CanSit)
				view.transform.SnapTo(sitNodeData.anchor);
		
		}
	}
}