using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	
	public class CharacterControllerNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode,ICharacterController
	{
		public DirectionalMovementInputNodeData DirectionalMovementInputNode { get; private set; } = new DirectionalMovementInputNodeData();
		public MovementNodeData MovementData { get; private set; } = new MovementNodeData(){speedMulti = 1};
		public MovementObserverNodeData MovementObserver { get; private set; } = new MovementObserverNodeData();

		private CharacterController _cc;
		public CharacterController CharacterController{get{return _cc;}}

		private ArrayNodeSignalReceiver receivers = new ArrayNodeSignalReceiver();
		
		public Vector3 force;
		public float decay = 0.9f;
		public Vector3 gravity;
		private Vector3 gravityAccumulation;
		public Vector3 rootPosition;

		public bool CanMoving = true;
		public bool CanRotating = true;
		public void Attached(INode node)
		{
			if (node is IOnUpdateUpdater uNode && node is IOnFixedUpdateUpdater fuNode && node is NodeView view)
			{
				_cc = view.GetComponent<CharacterController>();
				
				uNode.DOnUpdate.Subscribe(MyUpdate);
				fuNode.DOnFixedUpdate.Subscribe(MyFixedUpdate);
				
				Init(node);
			}
			
			
		}


		public void Detached(INode node)
		{
			if (node is IOnUpdateUpdater uNode && node is IOnFixedUpdateUpdater fuNode && node is NodeView view)
			{
				uNode.DOnUpdate.Unsubscribe(MyUpdate);
				fuNode.DOnFixedUpdate.Unsubscribe(MyFixedUpdate);

				receivers.InhibitAll();
			}
		}
		
		public void MyUpdate() {

			this.NotifySpeed();

			
		}
		
		public void MyFixedUpdate() {
			//rotate
            if (CanRotating) this.Rotate(DirectionalMovementInputNode.RotateDir);

			//calculate gravity
			gravityAccumulation += gravity *Time.deltaTime;
			if (CharacterController.isGrounded) {
				gravityAccumulation = gravity;
            }

            var moveDir = Vector3.zero; 

			if(CanMoving) moveDir +=	DirectionalMovementInputNode.MoveDir*MovementData.speedMulti;
			moveDir += force;
            moveDir += gravityAccumulation;

            moveDir *= Time.fixedDeltaTime;
			moveDir += rootPosition;

            force *=decay;
			if(force.sqrMagnitude<=0.01f){
				force = Vector3.zero;
			}
			
			CharacterController.Move(moveDir);

			rootPosition = Vector3.zero;
			
			
		}
		
		private void Init(INode root){
			DirectionalMovementInputNode = root.GetOrCreateNodeData<DirectionalMovementInputNodeData>(DirectionalMovementInputNode);

			MovementData = root.GetOrCreateNodeData<MovementNodeData>(MovementData);
			MovementObserver = root.GetOrCreateNodeData<MovementObserverNodeData>(MovementObserver);

			DirectionalMovementInputNode.DMove += (dir) => { DirectionalMovementInputNode.RotateDir = dir; };
			DirectionalMovementInputNode.DRotate += (dir) => { DirectionalMovementInputNode.MoveDir = dir; };
			
			//Initialize Gravity
			gravity =-9.8f*Vector3.up;
			
			//Add signal handler
			root.RegisterSignalReceiver<AddForceSignal>((x)=>{
				force += x.force;
			}).AddTo(receivers);
			
			//Add Stop handler
			root.RegisterSignalReceiver<StopForceSignal>((x)=>{
				force = Vector3.zero;
			}).AddTo(receivers);

            //Add Root Motion
            root.RegisterSignalReceiver<ApplyRootMotionPositionSignal>((x) => {
				rootPosition += x.position;
			}).AddTo(receivers);

            //Add Root Rotation
            root.RegisterSignalReceiver<ApplyRootMotionRotaionSignal>((x) => {
				CharacterController.transform.rotation = x.rotation;
            }).AddTo(receivers);

            //Add set moving active
            root.RegisterSignalReceiver<UpdateMovingSignal>((x) => {
				CanMoving = x.IsMoving;
			},true).AddTo(receivers);

            //Add set rotating active
            root.RegisterSignalReceiver<UpdateRotatingSignal>((x) => {
                CanRotating = x.IsRotating;
            }, true).AddTo(receivers);
        }	
	}
}