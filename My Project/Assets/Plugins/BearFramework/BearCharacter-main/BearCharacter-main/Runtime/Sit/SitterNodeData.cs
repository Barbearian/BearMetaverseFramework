using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class SitterNodeData : INodeData,IOnAttachedToNode
	{
		public void Attached(INode node){
			if(node.Can(MovementKeyword.StopMoving,SitterNodeDataKeyword.PlaySitAnimation)){
				if(node.TryGetNodeData<NaiveStateMachineNodeData>(out var sm)){
					var sitting = sm.GetOrCreateNaiveState(SitterNodeDataKeyword.Sitting);
					sitting.DOnEnterState+=()=>{Sit(node);};
				}
			}
		}
		
		
		public void Sit(INode node){
			node.InvokeAction(MovementKeyword.StopMoving);
			node.InvokeAction(SitterNodeDataKeyword.PlaySitAnimation);
		}


		
		
	}
	
	public static class SitterNodeDataKeyword{
		public const string PlaySitAnimation = "PlaySitAnimation";
		
		
		public const string Sitting = "Sitting";
	}
	
}