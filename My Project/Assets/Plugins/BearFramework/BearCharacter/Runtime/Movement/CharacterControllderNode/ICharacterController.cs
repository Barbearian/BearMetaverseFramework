using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	
	public interface IMovingObject{
				
		public DirectionalMovementInputNodeData DirectionalMovementInputNode { get;}
		public MovementNodeData MovementData { get; }
		public MovementObserverNodeData MovementObserver { get; } 
	}
	
	public interface ICharacterController:IMovingObject{
		public CharacterController CharacterController{get;}

	}
	
	public static class ICharacterControllerSystem{
		public static void Rotate(this ICharacterController controller,Vector3 forward){
			if (forward.sqrMagnitude > 0)
				controller.CharacterController.transform.forward  = forward;
		}
		
		public static void Move(this ICharacterController controller,Vector3 dir){
		    dir = controller.MovementData.speedMulti*dir;
			controller.CharacterController.SimpleMove(dir);
		}
		
		public static void NotifySpeed(this ICharacterController controller){
			float speed = controller.CharacterController.velocity.magnitude;
			controller.MovementObserver.DOnMove?.Invoke(speed);
		}
		
		public static void Stop(this ICharacterController controller){
			controller.Move(Vector3.zero);
		}
	}
}