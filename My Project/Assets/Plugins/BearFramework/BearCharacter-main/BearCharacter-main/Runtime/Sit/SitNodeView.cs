using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class SitNodeView : NodeView
	{
		public NodeView Anchor;
		private SitNodeData sitData;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			if(Anchor == null){
				Anchor = new GameObject("SitAnchor").AddNodeView<NodeView>();
				this.AddNodeViewChild(Anchor);
			}
			
			var anchorData = Anchor.GetOrCreateNodeData<TransformAnchorNodeData>(new TransformAnchorNodeData());
			this.GetOrCreateNodeData<SitNodeData>(new SitNodeData()).anchor =anchorData ;
		}
		
		
		
	}
	
	public static class SitNodeViewSystem{
		public static void SitOn(this NodeView view,NodeView Sit){
			if(Sit.TryGetNodeData<SitNodeData>(out var data)){
				view.SitOn(data);
			}
		}
	}
}