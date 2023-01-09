using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public abstract class AnimationEventListenerStartBehavior : AnimationEventListenerBehavior
	{
		public abstract IAnimationEventSignal GetSignal();
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var receiver = this.GetListener(animator);
			receiver.ReceiveSignal(GetSignal());
		}
	}
}