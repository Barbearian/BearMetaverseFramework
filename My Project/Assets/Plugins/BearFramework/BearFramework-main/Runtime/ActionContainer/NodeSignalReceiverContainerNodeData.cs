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
		
		public static void Deregister(this NodeSignalReceiverContainerNodeData data,string code,INodeSignalReceiver receiver = null){

			if (receiver != null)
			{
				if (
					data.nodeSignalReceivers.TryGetValue(code, out var oldReceiver) && 
					oldReceiver is ArrayNodeSignalReceiver areceiver
				)
				{
					areceiver.receivers.Remove(receiver);	
				}

            }
			else {
                data.nodeSignalReceivers.Remove(code);
            }
        }

		
        public static void ReceiveNodeSignal(this INode node,params INodeSignal[] signals){
			if(node.TryGetNodeData<NodeSignalReceiverContainerNodeData>(out var data)){
				foreach (var signal in signals)
				{
					data.ReceiveNodeSignal(signal);
				}
			}
		}

        public static void ReceiveNodeSignal<T>(this INode node, params INodeSignal[] signals)
        {
            if (node.TryGetNodeData<NodeSignalReceiverContainerNodeData>(out var data))
            {
                foreach (var signal in signals)
                {
                    data.ReceiveNodeSignal(typeof(T).ToString(),signal);
                }
            }
        }

		public static void ReceiveNodeSignal(this INode node, string key,params INodeSignal[] signals) {
            if (node.TryGetNodeData<NodeSignalReceiverContainerNodeData>(out var data))
            {
                foreach (var signal in signals)
                {
                    data.ReceiveNodeSignal(key, signal);
                }
            }
        }

        #region Reigster
        //Base Function
        public static void RegisterSignalReceiver(this INode node,string key,INodeSignalReceiver receiver,bool isAdditive = false){
            var actionPool = node.GetOrCreateNodeData<NodeSignalReceiverContainerNodeData>();
            if (isAdditive)
			{
				if (actionPool.nodeSignalReceivers.TryGetValue(key, out var oldreceiver) && oldreceiver is ArrayNodeSignalReceiver areceiver)
				{
					areceiver.receivers.Add(receiver);
				}
				else {
					actionPool.Register(key, new ArrayNodeSignalReceiver()
					{
						receivers = new List<INodeSignalReceiver>() {
							receiver
						}
					});

                }
            }
			else {
                actionPool.Register(key, receiver);

            }


        }
		
		public static void RegisterSignalReceiver<T>(this INode node,INodeSignalReceiver receiver, bool isAdditive = false)
		
		{
			var key = typeof(T).ToString();
			node.RegisterSignalReceiver(key,receiver,isAdditive);
		}
		
		public static ActionNodeSignalReceiver RegisterSignalReceiver<T>(this INode node,System.Action<T> action, bool isAdditive = false)
		{
			ActionNodeSignalReceiver receiver = new ActionNodeSignalReceiver((signal)=>{
				if(signal is T tsignal){
					action.Invoke(tsignal);
				}
			});
			
			node.RegisterSignalReceiver<T>(receiver, isAdditive);

			return receiver;
		}



        #endregion
    }
}