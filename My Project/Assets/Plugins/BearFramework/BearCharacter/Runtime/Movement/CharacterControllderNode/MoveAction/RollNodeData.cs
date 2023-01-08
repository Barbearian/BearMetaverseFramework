using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class RollNodeData:INodeData,IOnAttachedToNode
	{
		public float RollStrength;
		public NodeView root;
 		public void Attached(INode node){
	 		if(node is NodeView view){
	 			root = view;
	 		}
 		}
		
		public AddForceSignal GetForceSignal(){
			return new AddForceSignal(){
				force = root.transform.forward * RollStrength
			};
		}
	}
	
	public static class RollNodeDataSystem{
		public static void Roll(this RollNodeData node){
			node.root.ReceiveNodeSignal(node.GetForceSignal());
		}
	}
}