using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [RequireComponent(typeof(Animator))]
    public class AnimationRootMotionObserver : MonoBehaviour
    {
        Animator anim;
        public INode node;

        private ApplyRootMotionPositionSignal deltaPosition = new ApplyRootMotionPositionSignal();
        private ApplyRootMotionRotaionSignal deltaRotaion = new ApplyRootMotionRotaionSignal();
        public void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void OnAnimatorMove()
        {
            deltaPosition.position = anim.deltaPosition;
            deltaRotaion.rotation = anim.rootRotation;
            if (node != null) {
                node.ReceiveNodeSignal(deltaPosition);
                node.ReceiveNodeSignal(deltaRotaion);
            }

        }
    }

    public class ApplyRootMotionRotaionSignal : INodeSignal {
        public Quaternion rotation;
    }

    public class ApplyRootMotionPositionSignal: INodeSignal
    {
        public Vector3 position;
    }
}