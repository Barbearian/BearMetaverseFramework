using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInput;

namespace Bear{
	using System;
	using UnityEngine.InputSystem;
	public class SpeedUpNodeData : INodeData,IMiscActions,IOnAttachedToNode
	{
		public float speedMulti = 2;
		public Action<float> DOnSpeedUp;
		float origionalSpeed;
		private MovementNodeData movement;
		public void Attached(INode node){
			InputHelper.pInput.Misc.SetCallbacks(this);
			movement = node.GetOrCreateNodeData<MovementNodeData>();
			origionalSpeed = movement.speedMulti;
			DOnSpeedUp?.Invoke(movement.speedMulti);

			Debug.Log("Attched speedo up");
		}
		
		public void OnSpeedUp(UnityEngine.InputSystem.InputAction.CallbackContext context){
			
			if(context.phase == InputActionPhase.Performed){
				movement.speedMulti = origionalSpeed * speedMulti;
				DOnSpeedUp?.Invoke(movement.speedMulti);
				Debug.Log("Speed Up");
			}
            
			if(context.phase == InputActionPhase.Canceled){
				movement.speedMulti = origionalSpeed;
				DOnSpeedUp?.Invoke(movement.speedMulti);

				Debug.Log("Speed Down");


			}
		}
		
		
	}
}