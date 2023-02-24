
using UnityEngine.AI;

namespace Bear
{
	using UnityEngine;
    public class NavMeshAgentNodeData : INodeData, IOnAttachedToNode,IOnDetachedFromNode, INavMeshAgentController
    {
        public NavMeshAgent agent;

        public NavigatorInputNodeData PointInputNode { get; private set; } = new NavigatorInputNodeData();

        public DirectionalMovementInputNodeData DirectionalMovementInputNode { get; private set; } = new DirectionalMovementInputNodeData();

	    public MovementNodeData MovementData { get; private set; } = new MovementNodeData(){speedMulti = 1};

        public MovementObserverNodeData MovementObserver { get; private set; } = new MovementObserverNodeData();

        public NavMeshAgent Agent { get; private set; }
        private CharacterRotationLerp lerp;
        public void Attached(INode node)
        {
            if (node is IOnUpdateUpdater uNode && node is NodeView view)
            {
                Agent = view.GetComponent<NavMeshAgent>();

                uNode.DOnUpdate.Subscribe(MyUpdate);

                Init(node);
            }
        }


        public void Detached(INode node)
        {
            if (node is IOnUpdateUpdater uNode)
            {
                Agent = null;

	            uNode.DOnUpdate.Unsubscribe(MyUpdate);

            }
        }

        public void MyUpdate() {
            this.SnapTurn();
            this.CheckSpeed();
            this.NotifySpeed();

            this.Move(DirectionalMovementInputNode.MoveDir);

            lerp.SetTargetRotation(DirectionalMovementInputNode.RotateDir);
            this.Rotate(lerp.GetRotation());
        }

        public void Init(INode root) {
            PointInputNode = root.GetOrCreateNodeData<NavigatorInputNodeData>(PointInputNode);
            DirectionalMovementInputNode = root.GetOrCreateNodeData<DirectionalMovementInputNodeData>(DirectionalMovementInputNode);

            MovementData = root.GetOrCreateNodeData<MovementNodeData>(MovementData);
            MovementObserver = root.GetOrCreateNodeData<MovementObserverNodeData>(MovementObserver);

            DirectionalMovementInputNode.DMove += (dir) => { DirectionalMovementInputNode.RotateDir = dir; };
            DirectionalMovementInputNode.DRotate += (dir) => { DirectionalMovementInputNode.MoveDir = dir; };
            PointInputNode.DMoveTo += this.MoveTo;

            lerp = new CharacterRotationLerp()
            {
                RotationSpeed= 10,
                rotationTarget = agent.transform
            };
        }
        
	    public void MoveTo(Vector3 moveTo){
	    	this.MoveTo(moveTo,true);
	    }
    }
}