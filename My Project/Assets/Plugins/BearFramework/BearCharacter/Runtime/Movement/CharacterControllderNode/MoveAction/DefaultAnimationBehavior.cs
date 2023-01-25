using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear {
    public class DefaultAnimationBehavior : AnimationEventListenerBehavior
    {
        UpdateMovingSignal msignal = new UpdateMovingSignal();
        UpdateRotatingSignal rsignal = new UpdateRotatingSignal();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            msignal.IsMoving = true;
            rsignal.IsRotating = true;

            var listener = GetListener(animator);
            listener.ReceiveSignal(msignal);
            listener.ReceiveSignal(rsignal);
        }

    }
}