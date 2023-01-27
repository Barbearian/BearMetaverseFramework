using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class JumpNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		public float JumpStrength;
		public NodeView root;
        public SignalFilterReceiver filter = new SignalFilterReceiver();

        private ActionNodeSignalReceiver AddForceR;
		public void Attached(INode node){
			if(node is NodeView view){
				root = view;
				Init(node);
            }
		}

		private void Init(INode node) {
            //Assign output
            filter.Output = new ActionNodeSignalReceiver((x) => {
                node.ReceiveNodeSignal<IExecutableNodeSignal>(new JumpExecueSignal()
                {
                    node = root
                });
            });

            AddForceR = root.RegisterSignalReceiver<AddJumpForceSignal>((x) => {
                root.ReceiveNodeSignal(GetForceSignal());
            });

            node.Register("Move/Jump", (x) => {
                Debug.Log("I jumped");
				this.JumpActivate();

            });
        }

        public void Detached(INode node)
        {
			AddForceR.SetActive(false);
        }

        public AddForceSignal GetForceSignal(){
			var rs = new AddForceSignal();
			var force = root.transform.up * JumpStrength;
			rs.force = force;
			return rs;
		}
	}
	
	public static class JumpNodeDataSystem{

		public static void JumpActivate(this JumpNodeData node) {
			node.filter.Receive(new JumpActivateSignal());
		}
		public static void Jump(this JumpNodeData node){
			//node.root.ReceiveNodeSignal(node.GetForceSignal());

        }
	}

	public class AddJumpForceSignal:IAnimationEventSignal { 
	
	}

	public struct JumpActivateSignal : INodeSignal { 
	
	}

    public struct JumpExecueSignal : IExecutableNodeSignal, IMovementSignal
    {
		public INode node;
        public void Execute()
        {
            node.ReceiveNodeSignal(new AnimatorClipsPlayerNodeSignal()
            {
                info = new PlayAnimationClipInfo()
                {
                    clipName = "Jump",

                }
            });
        }
    }

    public class JumpFilter : ISignalFilter {
        public bool CanPass(INodeSignal signal = null)
        {
            if (signal is JumpExecueSignal jsignal)
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is RollFilter;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }
}