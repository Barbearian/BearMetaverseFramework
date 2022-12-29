
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Bear
{
    public static class NavMeshAgentControllerFactory
    {
        public static UpdaterNodeView AddNavMeshAgentCharacterNodeData(NavMeshAgent agent) {

            var view = agent.gameObject.AddNodeView<UpdaterNodeView>();

            //Add nanvData
            var nanvData = view.GetOrCreateNodeData(new NavMeshAgentNodeData());
	        view.RegisterAction(MovementKeyword.StopMoving,()=>{nanvData.Stop();});
            

            //Add state machine
            var naivesm = view.GetOrCreateNodeData(new NaiveStateMachineNodeData());

            //Trigger when move
	        nanvData.MovementObserver.DOnStartMove += () => { 
		        naivesm.EnterState("Moving"); 
	        };

			
	        
            return view;

        }

        public static void AddNavMeshAgentMovementInput(this UpdaterNodeView view) {
            var dirInput = view.GetOrCreateNodeData(new DirectionalMovementInputNodeData());
            var clickInput = view.GetOrCreateNodeData(new InputAssociateNodeData());
            var nanv = view.GetOrCreateNodeData(new NavMeshAgentNodeData());

            //directional input
            var directionalND = new DirectionalInputNodeData();
            directionalND = view.GetOrCreateNodeData(directionalND);

            MovementInputNode min = new MovementInputNode();
            directionalND.inputtarget.Link(min.Move);

            //associate movement
            min.forward += dirInput.DRotate;
            min.forward += dirInput.DMove;


            //click
            clickInput.Register("Player/ClickOnTarget", (x) =>
                {
                    nanv.MoveToMouseClick();
                }
            );
            
            
            
        }

	    public static NodeView AddAnimatorNodeData(this Animator anim, float maxSpeedBlend, string SpeedAttribute, string SpeedMultiAttribute) {
            var view = anim.gameObject.AddNodeView<NodeView>();

            var rs = view.GetOrCreateNodeData(new AnimatorNodeData());
            var data = view.GetOrCreateNodeData(new AnimatorMovementSpeedInputStreamReceiverNodeData());

            data.maxSpeedBlend = maxSpeedBlend;
            data.SpeedAttribute = SpeedAttribute;
            data.SpeedMultiAttribute = SpeedMultiAttribute;

            return view;
        }

        public static void LinkNavMeshAgentToAnimator(this NodeView controller, NodeView animatorView)
        {

            var animator = animatorView.GetOrCreateNodeData(new AnimatorNodeData());
            var receiver = animatorView.GetOrCreateNodeData(new AnimatorMovementSpeedInputStreamReceiverNodeData());
            //Add nanvData
            var nanvData = controller.GetOrCreateNodeData(new NavMeshAgentNodeData());
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
	            animator.Play(0);
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

	    public static void LinkInputToAnimator(this NodeView controller, NodeView animatorView,int count = 0,int shift = 0) {
            var inputData = controller.GetOrCreateNodeData(new InputAssociateNodeData());
            var data = animatorView.GetOrCreateNodeData(new AnimatorNodeData());
            var naivesm = controller.GetOrCreateNodeData(new NaiveStateMachineNodeData());

		    for (int i = 1+shift; i <= count+shift; i++)
            {
			    var key = "UI/UIShortCut" +(i-shift);
                var num = i - 1;
                inputData.Register(key, (x) => {
                    data.Play(num);
                    naivesm.EnterState("PlayStandingGesture");
                });
            }
        }

	    public static void AddGlobalPlayerControllerNodeData(this NodeView view){
	    	view.GetOrCreateNodeData<GlobalPlayerControllerNodeData>();
	    }

    }
}