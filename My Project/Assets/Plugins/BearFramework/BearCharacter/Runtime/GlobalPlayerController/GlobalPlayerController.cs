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
		private static InputNodeView input;
		public static bool TryGetInputNodeView(out InputNodeView view){
			
			if(input == null){
				if(player is NodeView nview){
					if(nview is InputNodeView iView){
						input = iView;
					}else{
						
						input = nview.GetComponentInChildren<InputNodeView>();
					}
					
				}
			}
			view = input;
			return view == null;
		}

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
		
		public static string GetState(){
			if(player != null){
				if(player.TryGetNodeData<NaiveStateMachineNodeData>(out var sm)){
					return sm.GetCurrentStateName();
				}
			}
			
			return "";
		}
		
		public static void PlayAnimation(int index){
			if(player != null){
				if(player.TryGetNodeData<AnimatorNodeData>(out var anim)){
					anim.Play(index);
				}
			}
		}

		public static void EnableMoving(bool enable){
			if(player != null){
				if(player.TryGetNodeData<DirectionalInputNodeData>(out var ddata)){
					ddata.SetEnable(enable);
				}
			}
		}
		

	}
}