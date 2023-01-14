using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class OnAirNodeData : INodeData, IOnAttachedToNode, IOnDetachedFromNode
    {
        private ActionNodeSignalReceiver OnAirR;
        private ActionNodeSignalReceiver OnGroundR;
        private INode node;
        private SetAnimatorBoolValueSignal onGroundSignal;

        public OnAirNodeData(string key) {
            onGroundSignal = new SetAnimatorBoolValueSignal() { key = key};
        }
        public void Attached(INode node)
        {
     
            OnAirR = node.RegisterSignalReceiver<LeaveGroundSignal>(DOnAir,true);
            OnGroundR = node.RegisterSignalReceiver<OnGroundSignal>(DOnGround,true);
            this.node = node;
        }

        private void DOnAir(LeaveGroundSignal signal) {
            onGroundSignal.value = false;
            node.ReceiveNodeSignal<IAnimatorProcessorSignal>(onGroundSignal);
        }

        private void DOnGround(OnGroundSignal signal)
        {
            onGroundSignal.value = true;
            node.ReceiveNodeSignal<IAnimatorProcessorSignal>(onGroundSignal);
        }

        public void Detached(INode node)
        {
            OnAirR.SetActive(false);
            OnGroundR.SetActive(false);
        }
    }
}