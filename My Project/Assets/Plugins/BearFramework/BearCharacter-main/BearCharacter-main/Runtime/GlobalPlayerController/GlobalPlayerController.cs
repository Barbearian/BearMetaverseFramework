using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class GlobalPlayerControllerNodeData:INodeData,IOnAttachedToNode,IOnDetachedFromNode{
		public void Attached(INode node){
			GlobalPlayerControllerSystem.player = node;
		}
		
		public void Detached(INode node){
			GlobalPlayerControllerSystem.player = null;

		}
	}
	
	public static class GlobalPlayerControllerSystem
	{
		public static INode player;
		public static void MoveTo(Vector3 location){
			if(player!=null){
				if(player.TryGetNodeData<NavMeshAgentNodeData>(out var data)){
					data.MoveTo(location);
				}
			}
		}
		
		public static void EnterState(string state){
			if(player!=null){
				if(player.TryGetNodeData<NaiveStateMachineNodeData>(out var data)){
					data.EnterState(state);
				}
			}
		}
		
		public static void SnapTo(IAnchorNodeData anchor){
			if(player!=null){
				
				player.ReceiveNodeSignal(new AnchorNodeSignal(anchor));
			}
		}
		

	}
}