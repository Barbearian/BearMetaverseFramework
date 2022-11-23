using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
using System;
	public static class NetworkedObjectSystem
	{
		public static Dictionary<string,NetworkedObjectNodeData> networkedObjects = new Dictionary<string, NetworkedObjectNodeData>();
		
		public static void SendData(this NetworkedObjectNodeData nobj,string key,string data){
			if(networkedObjects.TryGetValue(nobj.userID,out var value)){
				if(value.DOnReceiveMessages.TryGetValue(key,out var action)){
					action.Invoke(data);
				}
			}
		}
		
		public static void SubscribeData(this NetworkedObjectNodeData nobj,string key,Action<string> DOnReceiveData){
			nobj.DOnReceiveMessages[key] = DOnReceiveData;
		}
		
		public static void UnsubscribeData(this NetworkedObjectNodeData nobj,string key){
			nobj?.DOnReceiveMessages.Remove(key);
		}
		
		public static string GetID(this NetworkedObjectNodeData nobj){
			return nobj.userID+"_"+(int)nobj.type;
		}
		
	}
}