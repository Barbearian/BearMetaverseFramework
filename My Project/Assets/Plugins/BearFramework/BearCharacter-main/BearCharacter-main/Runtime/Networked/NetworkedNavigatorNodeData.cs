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
				nobj.SubscribeData(NetworkedNavigatorNodeDataSystem.key,(data)=>{
					var md = JsonUtility.FromJson<MoveData>(data);
					navi.MoveTo(md.position);
				});
				
			}
		}
		
		public void Detached(INode node){
			nobj.UnsubscribeData(NetworkedNavigatorNodeDataSystem.key);
		}
		
	}
	
	public static class NetworkedNavigatorNodeDataSystem{
		public const string key = "Move";
	}
	
	[System.Serializable]
	public struct MoveData{
		public Vector3 position;
	}
}