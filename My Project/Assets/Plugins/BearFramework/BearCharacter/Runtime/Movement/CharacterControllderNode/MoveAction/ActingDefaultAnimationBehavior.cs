using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ActingDefaultAnimationBehavior : AnimationEventListenerBehavior
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsActing",false);
            GetListener(animator).ReceiveSignal(new WielderToItemSignal() { 
                signal = new ItemEnterDefaultStateSignal()
            });


        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsActing", true);
        }
    }
}