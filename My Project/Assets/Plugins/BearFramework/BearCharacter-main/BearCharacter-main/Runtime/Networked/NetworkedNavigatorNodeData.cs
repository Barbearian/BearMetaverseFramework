using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class NetworkedNavigatorNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		
		public NavMeshAgentNodeData navi;
		NetworkedObjectNodeData nobj;
		public void Attached(INode node){
			
			if(node.TryGetNodeData<NavMeshAgentNodeData>(out navi) && node.TryGetNodeData<NetworkedObjectNodeData>(out nobj)){
				Init(node);

				
			}
		}
		
		public void Detached(INode node){
			nobj.UnsubscribeData(NetworkedNavigatorNodeDataSystem.key);
		}
		
		private void Init(INode node){
			switch(nobj.type){
				case NetworkedObjectType.local:
					nobj.SubscribeData(NetworkedNavigatorNodeDataSystem.key,(data)=>{
						var md = JsonUtility.FromJson<MoveData>(data);
						navi.MoveTo(md.position);
					});
					break;
				
				case NetworkedObjectType.networked:
					if(node is UpdaterNodeView view){
						Ticker ticker = new Ticker(Tick);	
						view.DOnFixedUpdate.Subscribe(ticker.Tick);
					}
					break;
			}

		}
		
		private void Tick(){
			
		}
		
	}
	
	public static class NetworkedNavigatorNodeDataSystem{
		public const string key = "Move";
	}
	
	[System.Serializable]
	public struct MoveData{
		public Vector3 position;
		public Quaternion rotation;
	}
}