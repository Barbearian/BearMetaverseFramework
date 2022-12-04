﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public static class CharacterControllerFactory 
	{
		public static UpdaterNodeView AddCharacterControllerNodeData(this CharacterController agent) {

			var view = agent.gameObject.AddNodeView<UpdaterNodeView>();

			//Add nanvData
			var nanvData = view.GetOrCreateNodeData(new CharacterControllerNodeData());
			view.RegisterAction(MovementKeyword.StopMoving,()=>{
				nanvData.Stop();
			});
            

			//Add state machine
			var naivesm = view.GetOrCreateNodeData(new NaiveStateMachineNodeData());

			//Trigger when move
			nanvData.MovementObserver.DOnStartMove += () => { 
				naivesm.EnterState("Moving"); 
			};

			
	        
			return view;

		}
		
		public static void AddCharacterControllerInput(this UpdaterNodeView view) {
			var dirInput = view.GetOrCreateNodeData(new DirectionalMovementInputNodeData());
			var nanv = view.GetOrCreateNodeData(new CharacterControllerNodeData());

			//directional input
			var directionalND = new DirectionalInputNodeData();
			directionalND = view.GetOrCreateNodeData(directionalND);

			MovementInputNode min = new MovementInputNode();
			directionalND.inputtarget.Link(min.Move);

			//associate movement
			min.forward += dirInput.DRotate;
			min.forward += dirInput.DMove;

            
            
		}
		
		public static void LinkCharacterControllerToAnimator(this NodeView controller, NodeView animatorView)
		{

			var animator = animatorView.GetOrCreateNodeData(new AnimatorNodeData());
			var receiver = animatorView.GetOrCreateNodeData(new AnimatorMovementSpeedInputStreamReceiverNodeData());
			//Add nanvData
			var nanvData = controller.GetOrCreateNodeData(new CharacterControllerNodeData());
			var movementObserver = controller.GetOrCreateNodeData(new MovementObserverNodeData());

			//update move speed, and add animator to controller's child 
			controller.AddNodeOrNodeViewChild(animatorView);
            
			movementObserver.DOnMove += (speed) => { receiver.UpdateSpeedAndMulti(speed); };

			//Update Moving
			var naivesm = controller.GetOrCreateNodeData(new NaiveStateMachineNodeData());
			var state = naivesm.GetOrCreateNaiveState("Moving");
			state.DOnEnterState += () =>
			{
				animatorView.transform.RestoreFromAnchor();
				animator.EnterDefaultState();
			};

			//stop moving when play guesture
			var playGesture = naivesm.GetOrCreateNaiveState("PlayStandingGesture");
			playGesture.DOnEnterState += () => {
				nanvData.Stop();
				Debug.Log("I tried to let player stop");
			};
            
            
			//add play to controller
			controller.RegisterSignalReceiver<AnimatorClipsPlayerNodeSignal>(new ActionNodeSignalReceiver((signal)=>{
				if(signal is AnimatorClipsPlayerNodeSignal asignal){
					var info = asignal.info;
					AnimatorNodeSystem.PlayInfo(animator,info);
				}
			}));
		}
	}
}