using static PlayerInput;
using UnityEngine.InputSystem;
using System.Diagnostics;
using UnityEngine;
namespace Bear
{
    public class ItemInputNodeData : INodeData, IItemActions, IOnAttachedToNode
    {
        private INode node;
        public void Attached(INode node)
        {
            this.node = node;
            InputHelper.pInput.Item.SetCallbacks(this);
        }

        public void OnLBH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new LBHPerformedSignal());
            else if(context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new LBHCancelSignal());
        }

        public void OnLBT(InputAction.CallbackContext context)
        {
    //        UnityEngine.Debug.Log("Hello");
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new LBTPerformedSignal());
        }

        public void OnLTH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new LTHPerformedSignal());
            else if (context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new LTHCancelSignal());
        }

        public void OnLTT(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new LTTPerformedSignal());
        }

        public void OnRBH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new RBHPerformedSignal());
            else if (context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new RBHCancelSignal());
        }

        public void OnRBT(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new RBTPerformedSignal());
        }

        public void OnRTH(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new RTHPerformedSignal());
            else if (context.phase == InputActionPhase.Canceled)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new RTHCancelSignal());
        }

        public void OnRTT(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                node.ReceiveNodeSignal<IPlayerToItemNodeSignal>(new RTTPerformedSignal());
        }
    }

    public struct LBHCancelSignal:IPlayerToItemNodeSignal { }
    public struct LBHPerformedSignal: IPlayerToItemNodeSignal { }
    public struct LBTPerformedSignal : IPlayerToItemNodeSignal { }
    public struct LTHCancelSignal: IPlayerToItemNodeSignal { }
    public struct LTHPerformedSignal: IPlayerToItemNodeSignal { }
    public struct LTTPerformedSignal: IPlayerToItemNodeSignal { }

    public struct RBHCancelSignal : IPlayerToItemNodeSignal { }
    public struct RBHPerformedSignal : IPlayerToItemNodeSignal { }
    public struct RBTPerformedSignal : IPlayerToItemNodeSignal { }
    public struct RTHCancelSignal : IPlayerToItemNodeSignal { }
    public struct RTHPerformedSignal : IPlayerToItemNodeSignal { }
    public struct RTTPerformedSignal : IPlayerToItemNodeSignal { }

}