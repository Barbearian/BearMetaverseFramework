using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class LayerNodeData : INodeData,IOnAttachedToNode
	{
		public NodeView view;
		public void Attached(INode node){
			if(node is NodeView view){
				this.view = view;
			}
		}
		
		public void ChangeLayer<T>(LayerMask mask,bool IncludingChild = false) 	where T:MonoBehaviour

		{
			if(view.gameObject.TryGetComponent<T>(out var viewobj)){
				viewobj.gameObject.layer = mask;
			}
			
			if(IncludingChild){
				foreach (var obj in view.gameObject.GetComponentsInChildren<T>())
				{
					obj.gameObject.layer = mask;
				}
			}
		}
		
		public void ChangeLayer(LayerMask mask,bool IncludingChild = false){
			if(view.gameObject.TryGetComponent<Transform>(out var viewobj)){
				viewobj.gameObject.layer = mask;
			}
			
			if(IncludingChild){
				foreach (var obj in view.gameObject.GetComponentsInChildren<Transform>())
				{
					obj.gameObject.layer = mask;
				}
			}		}
	}
}