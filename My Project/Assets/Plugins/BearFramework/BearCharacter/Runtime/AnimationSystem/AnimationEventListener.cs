using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AnimationEventListener : MonoBehaviour
	{
		public INode node;
		public Dictionary<string,INodeSignal> signals = new Dictionary<string, Bear.INodeSignal>();
		
		public void ReceiveSignal(INodeSignal signal){
			if(node!=null)
				node.ReceiveNodeSignal(signal);
		}
		
		public void ReceiveSignal(string keyword){
			if(signals.TryGetValue(keyword, out var signal)){
				ReceiveSignal(signal);
			}
		}
	}
	
	
}