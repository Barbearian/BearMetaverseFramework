
using UnityEngine;

namespace Bear
{
    public class ItemBehavior : AnimationEventListenerBehavior
    {
        AddFilterSignal movment = new AddFilterSignal()
        {
            filter = new ItemActFilter()
        };

        RemoveFilterSignal unmovment = new RemoveFilterSignal()
        {
            filter = new ItemActFilter()

        };


        UpdateMovingSignal msignal = new UpdateMovingSignal();
        UpdateRotatingSignal rsignal = new UpdateRotatingSignal();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            msignal.IsMoving = false;
            rsignal.IsRotating = false;

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