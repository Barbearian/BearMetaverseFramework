using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Bear{
	public class NetworkedNavigatorNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		public bool isUploadinig = false;
		public NavMeshAgentNodeData navi;
		NetworkedObjectNodeData nobj;
		MoveData data;
		NodeView transform;
		NodeView body;
		AnimatorNodeData anode;
		NaiveStateMachineNodeData nm;
		NavMeshAgent agent;
		bool Inited;
		
		public NetworkedNavigatorNodeData(NodeView body){
			this.body = body;
			anode = body.GetOrCreateNodeData<AnimatorNodeData>();

			
			
		}
		
		public void Attached(INode node){
			
			if(
				node.TryGetNodeData<NavMeshAgentNodeData>(out navi) && 
				node.TryGetNodeData<NetworkedObjectNodeData>(out nobj) &&
				
				node is NodeView view 
			){
			
				view.TryGetComponent<NavMeshAgent>(out agent); 
				transform = view;
				Init(node);
				
				
			}
		}
		
		public void Detached(INode node){
			nobj.UnsubscribeData(NetworkedNavigatorNodeDataSystem.key);
		}
		
		private void Init(INode node){
			switch(nobj.type){
				case NetworkedObjectType.local:
					anode.DOnPlayedIndexed.Link<int>(SendAnimationData);

					
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
					
					nobj.SubscribeData(NetworkedNavigatorNodeDataSystem.Animationkey,(data)=>{
						var md = JsonUtility.FromJson<MoveData>(data);
						PlayerAnimationClip(md);
					});
					
					if(node.TryGetNodeData<MovementObserverNodeData>(out var obs)){
						obs.DOnStop += ()=>{
							transform.transform.rotation = data.rotation;
						};
					}	
					break;
			}

		}
		
		private void Tick(){
			Debug.Log("Ticked");
			if(isUploadinig)
				nobj.SendData(NetworkedNavigatorNodeDataSystem.key,JsonUtility.ToJson(GetData()));
		}
		
		private void SendAnimationData(int index){
			var data = GetData();
			data.state = index;
			data.moving = false;
			if(isUploadinig)
				nobj.SendData(NetworkedNavigatorNodeDataSystem.Animationkey,JsonUtility.ToJson(data));

		}
		
		private MoveData GetData(){
			return new MoveData(){
				position = transform.transform.position,
				rotation = transform.transform.rotation,
				bodyPosition = body.transform.localPosition,
				bodyRotation = body.transform.localRotation,
				moving = nm.GetCurrentStateName().Equals(MovementKeyword.MovingState) || nm.GetCurrentStateName().Equals(SitterNodeDataKeyword.GoToSeat)
			};
		}
	
		
		public void SetDestination(MoveData data){
			
			

			this.data = data;

			if(data.moving){
				agent.updatePosition = true;
				navi.MoveTo(data.position,false);
			}else{
				agent.isStopped = true;
				agent.Move(data.position - transform.transform.position);
				agent.transform.rotation = data.rotation;

			}
			
			body.transform.localRotation = data.bodyRotation;
			body.transform.localPosition = data.bodyPosition;

		}
		
		public void PlayerAnimationClip(MoveData data){
			Debug.Log("哈");
			SetDestination(data);
			AnimatorNodeSystem.Play(anode,data.state);
		}
		
	}
	
	public static class NetworkedNavigatorNodeDataSystem{
		public const string key = "Move";
		public const string Animationkey = "PlayAnimation";
	}
	
	[System.Serializable]
	public struct MoveData{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 bodyPosition;
		public Quaternion bodyRotation;
		public bool moving;
		public int state;
	}
	

}