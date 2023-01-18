using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Bear{
	public class AnimationEventListenerOdinBehavior : SerializedStateMachineBehaviour
	{
		public List<INodeSignal> DOnEnter = new List<Bear.INodeSignal>();
		public List<INodeSignal> DOnExit = new List<Bear.INodeSignal>();
		AnimatorComponentFetcher fetcher = new AnimatorComponentFetcher(true);
		public AnimationEventListener GetListener(Animator anim){
			return fetcher.GetComponent<AnimationEventListener>(anim.gameObject);
		} 
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var receiver = GetListener(animator);
			foreach (var item in DOnEnter)
			{
				receiver.ReceiveSignal(item);
			}
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var receiver = GetListener(animator);
			foreach (var item in DOnExit)
			{
				receiver.ReceiveSignal(item);
			}
		}
		
		
	}
}