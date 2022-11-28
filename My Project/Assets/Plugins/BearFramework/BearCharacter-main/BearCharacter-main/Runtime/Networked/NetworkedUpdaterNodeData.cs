using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public abstract class NetworkedUpdaterNodeData : INodeData, IOnAttachedToNode
	{
		public virtual void Attached(INode node){
			if(node.TryGetNodeData<NetworkedObjectNodeData>(out var nodeData)){
				Init(node,nodeData);
			}
		}
		
		public virtual void NetworkedUpdate(){
			
		}
		
		public void SendNetworkedData(string key,string data){
			
		}
		
		public abstract void Init(INode node,NetworkedObjectNodeData networkedData);
	}
}