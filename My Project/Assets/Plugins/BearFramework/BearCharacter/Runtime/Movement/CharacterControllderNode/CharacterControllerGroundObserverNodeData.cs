
using UnityEngine;

namespace Bear
{
    public class CharacterControllerGroundObserverNodeData : INodeData, IOnAttachedToNode, IOnDetachedFromNode
    {

        private CharacterController _cc;
        public CharacterController CharacterController { get { return _cc; } }
        public bool isGround;
        private INode node;
        private OnGroundSignal OnGroundSignal { get; set; } = new OnGroundSignal();
        private LeaveGroundSignal LeaveGroundSignal { get; set; } = new LeaveGroundSignal();

        public void Attached(INode node)
        {
            if (node is IOnFixedUpdateUpdater fuNode && node is NodeView view)
            {
                _cc = view.GetComponent<CharacterController>();

                fuNode.DOnFixedUpdate.Subscribe(MyFixedUpdate);

                Init(node);
            }

        }
        public void Detached(INode node)
        {
            if (node is IOnFixedUpdateUpdater fuNode && node is NodeView view)
            {
                fuNode.DOnFixedUpdate.Unsubscribe(MyFixedUpdate);
            }
        }

        public void MyFixedUpdate()
        {
            var newIsGround = CharacterController.isGrounded;
            if (isGround != newIsGround) {
                if (newIsGround)
                {
                    Debug.Log("On ground");
                    node.ReceiveNodeSignal(OnGroundSignal);
                }
                else {
                    Debug.Log("Leave ground");
                    node.ReceiveNodeSignal(LeaveGroundSignal);
                }
                
            }

            isGround = newIsGround;

        }

        private void Init(INode root)
        {
            node = root;
            OnGroundSignal.nodeData = this;
            LeaveGroundSignal.nodeData = this;
        }
    }

    public class LeaveGroundSignal : INodeSignal {
        public CharacterControllerGroundObserverNodeData nodeData;
    }

    public class OnGroundSignal : INodeSignal {
        public CharacterControllerGroundObserverNodeData nodeData;
    }
}