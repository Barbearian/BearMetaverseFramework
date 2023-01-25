using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

namespace Bear
{
	[Serializable]
    public class InputNodeView : NodeView,IPlayerActions
    {
        public SafeDelegate<Vector2> inputtarget = new SafeDelegate<Vector2>(); 
        public InputAssociateNodeData buttonInputData = new InputAssociateNodeData();
        public Vector2 dir;
        private void OnEnable() {
            InputHelper.pInput.Player.SetCallbacks(this);
        }

	    public void SetEnable(bool isEnabled){
		    if(isEnabled)
			    InputHelper.pInput.Enable();
		    else
			    InputHelper.pInput.Disable();

		    dir = Vector3.zero;
	    }

        public void OnClickOnTarget(InputAction.CallbackContext context)
        {}

        public void OnDoubleClick(InputAction.CallbackContext context)
        {}

        public void OnMoveDir(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed){
                dir = context.ReadValue<Vector2>();
            }
            
            if(context.phase == InputActionPhase.Canceled){
                dir = Vector2.zero;
            }
            
        }

        private void Update() {
            inputtarget.invoker?.Invoke(dir);
        }

        private void OnDestroy() {
            buttonInputData.Dispose();
        }
    }

	public class InputAssociateNodeData:INodeData,IOnDetachedFromNode,IOnAttachedToNode{
        public Dictionary<string,List<System.Action<InputAction.CallbackContext>>> actions = new Dictionary<string, List<System.Action<InputAction.CallbackContext>>>();
		public void Attached(INode node){
			Debug.Log("I am attached");
		}
		
		public void Detached(INode node){
			Debug.Log("I am detached");

			this.Dispose();
		}
	}

    public static class InputAssociateNodeDataSystem{
        public static void Register(this InputAssociateNodeData nodedata, string code, System.Action<InputAction.CallbackContext> DOnPerformed){
            var inputaction = InputHelper.GetAction(code);
            if(inputaction != null){
                inputaction.performed += DOnPerformed;
                nodedata.actions.Enqueue(code,DOnPerformed);    
            }
        }

        public static void Deregister(this InputAssociateNodeData nodedata, string code, System.Action<InputAction.CallbackContext> DOnPerformed){
            var inputaction = InputHelper.GetAction(code);
            if(inputaction != null){
                inputaction.performed -= DOnPerformed;
                nodedata.actions.Dequeue(code,DOnPerformed);    
            }
        }

        public static void Dispose(this InputAssociateNodeData nodedata){
            var dic = nodedata.actions;
            nodedata.actions = new Dictionary<string, List<System.Action<InputAction.CallbackContext>>>();
            foreach(var kv in dic){
                foreach(var action in kv.Value)
                    nodedata.Deregister(kv.Key,action);
            }
        }

        public static void Register(this INode node, string code, System.Action<InputAction.CallbackContext> DOnPerformed) {
            var nodedata = node.GetOrCreateNodeData(new InputAssociateNodeData());
            nodedata.Register(code, DOnPerformed);
            
        }

        public static void Deregister(this INode node, string code, System.Action<InputAction.CallbackContext> DOnPerformed)
        {
            var nodedata = node.GetOrCreateNodeData(new InputAssociateNodeData());
            nodedata.Deregister(code, DOnPerformed);

        }

        public static void DisposeInputAssociator(this INode node)
        {
            var nodedata = node.GetOrCreateNodeData(new InputAssociateNodeData());
            nodedata.Dispose();

        }
    }


}