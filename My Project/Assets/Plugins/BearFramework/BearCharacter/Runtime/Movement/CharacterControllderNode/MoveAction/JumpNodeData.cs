using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class JumpNodeData : INodeData,IOnAttachedToNode
	{
		public float JumpStrength;
		public NodeView root;
		public void Attached(INode node){
			if(node is NodeView view){
				root = view;
			}
		}
		
		public AddForceSignal GetForceSignal(){
			var rs = new AddForceSignal();
			var force = root.transform.up * JumpStrength;
			rs.force = force;
			return rs;
		}
	}
	
	public static class JumpNodeDataSystem{
		public static void Jump(this JumpNodeData node){
			node.root.ReceiveNodeSignal(node.GetForceSignal());
		}
	}
}