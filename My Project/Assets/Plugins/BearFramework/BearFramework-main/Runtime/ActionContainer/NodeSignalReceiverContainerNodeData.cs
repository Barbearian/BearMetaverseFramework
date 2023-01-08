using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class NodeSignalReceiverContainerNodeData : INodeData,IOnDetachedFromNode
	{
		public Dictionary<string,INodeSignalReceiver> nodeSignalReceivers = new Dictionary<string, INodeSignalReceiver>();

		public void Detached(INode node){
			nodeSignalReceivers.Clear();
		}
	}
	
	public static class NodeSignalReceiverContainerNodeDataystem 
	{
		public static void ReceiveNodeSignal(this NodeSignalReceiverContainerNodeData data,string code,INodeSignal signal){
			if(data.nodeSignalReceivers.TryGetValue(code, out var receiver)){
				if(receiver.IsActive){
					receiver.Receive(signal);
				}else{
					data.Deregister(code);
				}
			}
		}
		
		public static void ReceiveNodeSignal(this NodeSignalReceiverContainerNodeData data,INodeSignal signal){
			var code = signal.GetType().ToString();
			data.ReceiveNodeSignal(code,signal);
		}
		
		public static void Register(this NodeSignalReceiverContainerNodeData data,string code,INodeSignalReceiver action){
			data.nodeSignalReceivers[code] = action;
		}
		
		public static void Deregister(this NodeSignalReceiverContainerNodeData data,string code){
			data.nodeSignalReceivers.Remove(code);
		}

		
		public static void ReceiveNodeSignal(this INode node,params INodeSignal[] signals){
			if(node.TryGetNodeData<NodeSignalReceiverContainerNodeData>(out var data)){
				foreach (var signal in signals)
				{
					data.ReceiveNodeSignal(signal);
				}
			}
		}
		
		public static void RegisterSignalReceiver(this INode node,string key,INodeSignalReceiver receiver){
			var actionPool = node.GetOrCreateNodeData<NodeSignalReceiverContainerNodeData>();
			actionPool.Register(key,receiver);
		}
		
		public static void RegisterSignalReceiver<T>(this INode node,INodeSignalReceiver receiver) where T:INodeSignal
		
		{
			var key = typeof(T).ToString();
			node.RegisterSignalReceiver(key,receiver);
		}
		
		public static void RegisterSignalReceiver<T>(this INode node,System.Action<T> action) where T:INodeSignal
		{
			ActionNodeSignalReceiver receiver = new ActionNodeSignalReceiver((signal)=>{
				if(signal is T tsignal){
					action.Invoke(tsignal);
				}
			});
			
			node.RegisterSignalReceiver<T>(receiver);
		}
		
	}
}