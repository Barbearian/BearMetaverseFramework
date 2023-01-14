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
	 			Init();
	 		}
 		}
 		
		private void Init(){
			root.RegisterSignalReceiver<RollSignal>((signal)=>{
				if(!signal.IsEnd){
					this.Roll(signal.multiplier);

				}else{
					this.StopRoll();
				}
			});
		}
		
		public AddForceSignal GetForceSignal(float multiplier = 1){
			return new AddForceSignal(){
				force = root.transform.forward * RollStrength * multiplier
			};
		}
	}
	
	public static class RollNodeDataSystem{
		public static void Roll(this RollNodeData node,float multiplier = 1){
			node.root.ReceiveNodeSignal(node.GetForceSignal(multiplier));
		}
		
		public static void StopRoll(this RollNodeData node){
			node.root.ReceiveNodeSignal(new StopForceSignal());
		}
	}
	
	public class RollSignal:IAnimationEventSignal{
		public bool IsEnd;
		public float multiplier = 1;
	}
}