using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Bear{
	public class AnimationEventListenerOdinBehavior : SerializedStateMachineBehaviour
	{
		public List<IAnimationEventSignal> DOnEnter = new List<Bear.IAnimationEventSignal>();
		public List<IAnimationEventSignal> DOnExit = new List<Bear.IAnimationEventSignal>();
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