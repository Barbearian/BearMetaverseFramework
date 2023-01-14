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
		
		public Vector3 force;
		public float decay = 0.9f;
		public Vector3 gravity;
		public Vector3 rootPosition;
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
			}
		}
		
		public void MyUpdate() {

			this.NotifySpeed();

			this.Rotate(DirectionalMovementInputNode.RotateDir);
		}
		
		public void MyFixedUpdate() {

			//this.Move(DirectionalMovementInputNode.MoveDir);
			var moveDir = DirectionalMovementInputNode.MoveDir*MovementData.speedMulti;
			moveDir += gravity+force;
			
			force*=decay;
			if(force.sqrMagnitude<=0.1f){
				force = Vector3.zero;
			}
			
			CharacterController.Move(moveDir*Time.fixedDeltaTime + rootPosition);

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
			});
			
			//Add Stop handler
			root.RegisterSignalReceiver<StopForceSignal>((x)=>{
				force = Vector3.zero;
			});

			//Add Root Motion
			root.RegisterSignalReceiver<ApplyRootMotionPositionSignal>((x) => {
				rootPosition += x.position;
			});

            //Add Root Rotation
            root.RegisterSignalReceiver<ApplyRootMotionRotaionSignal>((x) => {
				CharacterController.transform.rotation = x.rotation;
            });
        }	
	}
}