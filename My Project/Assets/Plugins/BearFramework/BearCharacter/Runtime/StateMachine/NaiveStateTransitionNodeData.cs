using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class NaiveStateTransitionNodeData : INodeData,IOnAttachedToNode
	{
		
		public NaiveStateMachineNodeData nsm;
		public Dictionary<string,NaiveStateTransition> transitions = new Dictionary<string,NaiveStateTransition>();
		public void Attached(INode node){
			nsm = node.GetOrCreateNodeData<NaiveStateMachineNodeData>();
		}
		
		//public void Detached(INode node){
			
		//}
	}
	
	public static class NaiveStateTransitionNodeDataSystem{
		public static bool TryTransite(this NaiveStateTransitionNodeData transitions,string key,out string newState){
			var currentState = transitions.nsm.CurrentStateName;
			if(transitions.transitions.TryGetValue(currentState,out var transition)){
				return transition.Transitions.TryGetValue(key,out newState);
			}
			
			newState = null;
			return false;
		}
	}
	
	public class NaiveStateTransition{
		public Dictionary<string,string> Transitions = new Dictionary<string, string>();
	}
}