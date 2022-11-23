using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class NetworkedObjectNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		public Dictionary<string,Action<string>> DOnReceiveMessages = new Dictionary<string,Action<string>>();
		public string userID;
		public NetworkedObjectType type= NetworkedObjectType.local;
		public NetworkedObjectNodeData(string userID,NetworkedObjectType type = NetworkedObjectType.local){
			this.userID = userID;
		}
		
		
		public void Attached(INode node){
			NetworkedObjectSystem.networkedObjects[this.GetID()] = this; 
		}
		
		public void Detached(INode node){
			NetworkedObjectSystem.networkedObjects.Remove(this.GetID());
		}
		
	}
	
	public enum NetworkedObjectType{
		local = 0,
		networked = 1
	}
}