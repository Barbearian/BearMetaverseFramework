using static PlayerInput;
using UnityEngine.InputSystem;
namespace Bear
{
    public class ItemInputNodeData : INodeData, IItemActions, IOnAttachedToNode
    {
        private INode node;
        public void Attached(INode node)
        {
            this.node = node;
        }

        public void OnLBH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new LBHPerformedSignal());
            else if(context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal(new LBHCancelSignal());
        }

        public void OnLBT(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new LBTPerformedSignal());
        }

        public void OnLTH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new LTHPerformedSignal());
            else if (context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal(new LTHCancelSignal());
        }

        public void OnLTT(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new LTTPerformedSignal());
        }

        public void OnRBH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new RBHPerformedSignal());
            else if (context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal(new RBHCancelSignal());
        }

        public void OnRBT(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new RBTPerformedSignal());
        }

        public void OnRTH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new RTHPerformedSignal());
            else if (context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal(new RTHCancelSignal());
        }

        public void OnRTT(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal(new RTTPerformedSignal());
        }
    }

    public struct LBHCancelSignal:INodeSignal { }
    public struct LBHPerformedSignal:INodeSignal { }
    public struct LBTPerformedSignal : INodeSignal { }
    public struct LTHCancelSignal:INodeSignal { }
    public struct LTHPerformedSignal:INodeSignal { }
    public struct LTTPerformedSignal:INodeSignal { }

    public struct RBHCancelSignal : INodeSignal { }
    public struct RBHPerformedSignal : INodeSignal { }
    public struct RBTPerformedSignal : INodeSignal { }
    public struct RTHCancelSignal : INodeSignal { }
    public struct RTHPerformedSignal : INodeSignal { }
    public struct RTTPerformedSignal : INodeSignal { }

}