using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class SitNodeView : NodeView
	{
		public NodeView Anchor;
		private SitNodeData sitData;
		
		// Awake is called when the script instance is being loaded.
		public override void Awake()
		{
			base.Awake();
			if(Anchor == null){
				Anchor = new GameObject("SitAnchor").AddNodeView<NodeView>();
				this.AddNodeViewChild(Anchor);
			}
			
			var anchorData = Anchor.GetOrCreateNodeData<TransformAnchorNodeData>(new TransformAnchorNodeData());
			
			sitData = this.GetOrCreateNodeData<SitNodeData>(new SitNodeData());
			sitData.anchor = anchorData;
		}
		
		public void GoToSit(){
			GlobalPlayerControllerSystem.MoveTo(transform.position);
		}
		
		public void Sit(){
			GlobalPlayerControllerSystem.EnterState(SitterNodeDataKeyword.Sitting);
			GlobalPlayerControllerSystem.SnapTo(sitData.anchor);
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