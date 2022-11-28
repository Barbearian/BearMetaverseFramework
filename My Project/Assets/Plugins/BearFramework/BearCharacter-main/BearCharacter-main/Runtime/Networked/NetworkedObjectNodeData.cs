using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class NetworkedObjectNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		public Dictionary<string,Action<string>> DOnReceiveMessages = new Dictionary<string,Action<string>>();
		public string userID;
		public List<string> data;
		public NetworkedObjectType type= NetworkedObjectType.local;
		public NetworkedObjectNodeData(string userID,NetworkedObjectType type = NetworkedObjectType.local){
			this.userID = userID;
			this.type = type;
		}
		
		
		public void Attached(INode node){
			NetworkedObjectSystem.networkedObjects[this.GetID(this.type)] = this; 
		}
		
		public void Detached(INode node){
			NetworkedObjectSystem.networkedObjects.Remove(this.GetID(this.type));
		}
		
	}
	
	public enum NetworkedObjectType{
		unknow = 0,
		local = 1,
		networked = 2
	}
}