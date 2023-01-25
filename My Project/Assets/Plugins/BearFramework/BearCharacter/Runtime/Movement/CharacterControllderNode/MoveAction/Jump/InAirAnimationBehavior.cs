using UnityEngine;

namespace Bear
{
    public class InAirAnimationBehavior : AnimationEventListenerBehavior
    {

        AddFilterSignal movment = new AddFilterSignal()
        {
            filter = new MovementSignalFilter()
        };

        RemoveFilterSignal unmovment = new RemoveFilterSignal()
        {
            filter = new MovementSignalFilter()

        };


        UpdateMovingSignal msignal = new UpdateMovingSignal();
        UpdateRotatingSignal rsignal = new UpdateRotatingSignal();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            msignal.IsMoving = true;
            rsignal.IsRotating = true;

            var listener = GetListener(animator);
            listener.ReceiveSignal(movment);

            listener.ReceiveSignal(msignal);
            listener.ReceiveSignal(rsignal);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            var listener = GetListener(animator);
            listener.ReceiveSignal(unmovment);


        }
    }
}