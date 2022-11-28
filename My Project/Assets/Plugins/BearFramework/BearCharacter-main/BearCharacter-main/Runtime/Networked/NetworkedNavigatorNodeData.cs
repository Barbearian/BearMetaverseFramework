using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class NetworkedNavigatorNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		
		public NavMeshAgentNodeData navi;
		NetworkedObjectNodeData nobj;
		MoveData data;
		Transform transform;
		NaiveStateMachineNodeData nm;
		public void Attached(INode node){
			
			if(
				node.TryGetNodeData<NavMeshAgentNodeData>(out navi) && 
				node.TryGetNodeData<NetworkedObjectNodeData>(out nobj) &&
				node is NodeView view
			){
				transform = view.transform;
				Init(node);
				
				
			}
		}
		
		public void Detached(INode node){
			nobj.UnsubscribeData(NetworkedNavigatorNodeDataSystem.key);
		}
		
		private void Init(INode node){
			switch(nobj.type){
				case NetworkedObjectType.local:

					
					if(node is UpdaterNodeView view){
						Ticker ticker = new Ticker(Tick);	
						view.DOnFixedUpdate.Subscribe(ticker.Tick);
						
						view.TryGetNodeData<NaiveStateMachineNodeData>(out nm);
					}
					
					break;
				
				case NetworkedObjectType.networked:
					nobj.SubscribeData(NetworkedNavigatorNodeDataSystem.key,(data)=>{
						var md = JsonUtility.FromJson<MoveData>(data);
						SetDestination(md);
					});
					
					if(node.TryGetNodeData<MovementObserverNodeData>(out var obs)){
						obs.DOnStop += ()=>{
							transform.rotation = data.rotation;
						};
					}	
					break;
			}

		}
		
		private void Tick(){
			Debug.Log("Ticked");
			nobj.SendData(NetworkedNavigatorNodeDataSystem.key,JsonUtility.ToJson(new MoveData(){
				position = transform.position,
				rotation = transform.rotation,
				moving = nm.GetCurrentStateName().Equals(MovementKeyword.MovingState)
			}));
		}
		
		public void SetDestination(MoveData data){
			this.data = data;
			navi.MoveTo(data.position);

		}
		
	}
	
	public static class NetworkedNavigatorNodeDataSystem{
		public const string key = "Move";
	}
	
	[System.Serializable]
	public struct MoveData{
		public Vector3 position;
		public Quaternion rotation;
		public bool moving;
	}
}