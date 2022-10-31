using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class AnimatorSignalNotifier : StateMachineBehaviour
    {

        bool inited;
        ISignalDistributor _distributor;
        public ISignalDistributor GetDistributor(Animator animator) {
            if (inited) {
                return _distributor;
            }
            inited = true;

            if (animator.TryGetComponent(out _distributor))
            {
                return _distributor;
            }
            else {
                return animator.gameObject.AddComponent<ISignalDistributor>();
            }
        }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }


    }
}