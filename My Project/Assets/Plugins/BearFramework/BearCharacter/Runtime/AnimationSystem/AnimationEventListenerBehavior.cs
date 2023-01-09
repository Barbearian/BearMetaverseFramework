using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AnimationEventListenerBehavior : StateMachineBehaviour
	{
		AnimatorComponentFetcher fetcher = new AnimatorComponentFetcher(true);
		public AnimationEventListener GetListener(Animator anim){
			return fetcher.GetComponent<AnimationEventListener>(anim.gameObject);
		} 
		
		
	}
	
	
	public interface IAnimationEventSignal:INodeSignal{}
	
}