using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	
	public interface IAnchorNodeData:INodeData{
		public Vector3 Position{get;set;}
		public Quaternion Rotation{get;set;}
	}
	
	public struct AnchorNodeData : IAnchorNodeData
	{
		
		public Vector3 Position{get;set;}
		public Quaternion Rotation{get;set;}
	}
	
	public struct TransformAnchorNodeData: IAnchorNodeData,IOnAttachedToNode,IOnDetachedFromNode{
		public Transform transform;
		public Vector3 Position{get => transform.position;set{transform.position = value;}}
		public Quaternion Rotation{get => transform.rotation;set{transform.rotation = value;}}
		
		public void Attached(INode node){
			if(node is NodeView view){
				this.transform = view.transform;
			}
		}
		
		public void Detached(INode node){
			transform = null;
		}
	}
	
	public static class AnchorNodeDataSystem{
		public static void SnapTo(this Transform transform,IAnchorNodeData data){
			transform.position = data.Position;
			transform.rotation = data.Rotation;
		}
		
		public static void RestoreFromAnchor(this Transform transform){
			transform.localPosition = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}
	}
}