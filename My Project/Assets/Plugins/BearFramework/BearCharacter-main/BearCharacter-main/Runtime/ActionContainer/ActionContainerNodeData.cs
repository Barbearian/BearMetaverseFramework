using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class ActionContainerNodeData : INodeData,IOnDetachedFromNode
	{
		public Dictionary<string,Action> actionPool = new Dictionary<string, Action>();

		public void Detached(INode node){
			actionPool.Clear();
		}
		
	}
	
	public static class ActionContainerNodeDataSystem 
	{
		public static void Invoke(this ActionContainerNodeData data,string code){
			if(data.actionPool.TryGetValue(code, out var action)){
				action?.Invoke();
			}
		}
		
		public static void Register(this ActionContainerNodeData data,string code,Action action){
			data.actionPool[code] = action;
		}
		
		public static void Deregister(this ActionContainerNodeData data,string code){
			data.actionPool.Remove(code);
		}
		
		public static bool Can(this ActionContainerNodeData data,params string[] can){
			foreach (var item in can)
			{
				if(!data.actionPool.ContainsKey(item)) return false;
			}
			return true;
		}
		
		public static bool Can(this INode node,params string[] can){
			if(node.TryGetNodeData<ActionContainerNodeData>(out var data)){
				return data.Can(can);
			}
			return false;
		}
		
		public static void InvokeAction(this INode node,params string[] actions){
			if(node.TryGetNodeData<ActionContainerNodeData>(out var data)){
				foreach (var item in actions)
				{
					data.Invoke(item);
				}
			}
		}
		
		public static void RegisterAction(this INode node,string key,Action action){
			var actionPool = node.GetOrCreateNodeData<ActionContainerNodeData>();
			actionPool.Register(key,action);
		}
		
	}
}