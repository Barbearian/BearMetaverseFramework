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
					sitting.DOnEnterState+=()=>{
						Sit(node);
					};
					
					var GoToSeat = sm.GetOrCreateNaiveState(SitterNodeDataKeyword.GoToSeat);
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
		
		public const string GoToSeat = "GoToSeat";
	}
	
	public static class SitterNodeDataSystem{
		public static void AddSitterNodeData(NodeView controller,NodeView animator,int sitIndex = -1){
			//add sit
			controller.AddNodeData(new SitterNodeData());
			
			if(sitIndex >= 0){
				if(animator.TryGetNodeData<AnimatorNodeData>(out var adata)){
					controller.RegisterAction(SitterNodeDataKeyword.PlaySitAnimation,()=>{adata.Play(sitIndex);});
				}

			}else{
				if(animator.TryGetNodeData<AnimatorNodeData>(out var adata)){
					controller.RegisterAction(SitterNodeDataKeyword.PlaySitAnimation,()=>{adata.Play("Sit",0,0);});
				}
			}
			controller.RegisterSignalReceiver<AnchorNodeSignal>(new ActionNodeSignalReceiver((signal)=>{
				if(signal is AnchorNodeSignal asignal){
					Debug.Log("I snapped this boy");
					animator.transform.SnapTo(asignal.data);

				}
			}));
			
			
			
			controller.AddNodeData(new SitterNodeData());
		}
	}
	
}