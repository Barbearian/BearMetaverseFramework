using System.Collections;
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
		
		public static void AddJumpAndRoll(this UpdaterNodeView view){
			//Add nanvData
			var nanvData = view.GetOrCreateNodeData(new CharacterControllerNodeData());
			
			var inputData = view.GetOrCreateNodeData(new InputAssociateNodeData());
			
			var roll = view.GetOrCreateNodeData<RollNodeData>();
			var jump = view.GetOrCreateNodeData<JumpNodeData>();
			roll.RollStrength = 20;
			jump.JumpStrength = 50;
			inputData.Register("MoveAction/Roll",(x)=>{
				Debug.Log("I rolled");
				//roll.Roll();
				view.ReceiveNodeSignal(new AnimatorClipsPlayerNodeSignal(){
					info = new PlayAnimationClipInfo(){
						clipName = "Roll",
						
					}
				});
			});
			
			inputData.Register("MoveAction/Jump",(x)=>{
				Debug.Log("I jumped");
				jump.Jump();
                view.ReceiveNodeSignal(new AnimatorClipsPlayerNodeSignal()
                {
                    info = new PlayAnimationClipInfo()
                    {
                        clipName = "Jump",

                    }
                });
            });

			//add on air
			view.AddNodeData(new OnAirNodeData("IsGround"));
			view.AddNodeData(new CharacterControllerGroundObserverNodeData());
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
			
			
			//add animation event listener
			var listener = animatorView.gameObject.AddComponent<AnimationEventListener>();
			listener.node = controller;

			//add Root motion listener
			var observer =  animatorView.gameObject.AddComponent<AnimationRootMotionObserver>();
			observer.node = controller;

			//add animator signal Prosessor
			animatorView.AddNodeData(new AnimatorProcessorNodeData(controller));
		}
	}
}