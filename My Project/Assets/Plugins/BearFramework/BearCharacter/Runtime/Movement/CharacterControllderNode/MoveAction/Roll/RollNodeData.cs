using Codice.Client.BaseCommands.Merge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class RollNodeData:INodeData,IOnAttachedToNode
	{
		public float RollStrength;
		public NodeView root;
		public SignalFilterReceiver filter = new SignalFilterReceiver();
 		public void Attached(INode node){
	 		if(node is NodeView view){
	 			root = view;
	 			Init(node);
	 		}
 		}
 		
		private void Init(INode node)
        {
            //Assign output
			filter.Output = new ActionNodeSignalReceiver(RollActivated);

            node.Register("Move/Roll", (x) => {
                filter.Receive(new RollActivateSignal());
            });

        }

		private void RollActivated(INodeSignal signal) {
            root.ReceiveNodeSignal<IExecutableNodeSignal>(new RollExecuteSignal()
            {
                node = root
            });
		}
	}
	
	public static class RollNodeDataSystem{
		public static void RollActivate(this RollNodeData node,float multiplier = 1){

            node.filter.Receive(new RollActivateSignal());
        }

		public static void RollExcute(this RollNodeData node) {
            node.root.ReceiveNodeSignal(new AnimatorClipsPlayerNodeSignal()
            {
                info = new PlayAnimationClipInfo()
                {
                    clipName = "Roll",

                }
            });
        }
		
	}

    public struct RollActivateSignal : IAnimationEventSignal
    {

    }

    public struct RollExecuteSignal : IExecutableNodeSignal, IMovementSignal
    {
        public INode node;
        public void Execute()
        {
            node.ReceiveNodeSignal(new AnimatorClipsPlayerNodeSignal()
            {
                info = new PlayAnimationClipInfo()
                {
                    clipName = "Roll",

                }
            });
        }
    }

    public class RollFilter : ISignalFilter
    {
        public bool CanPass(INodeSignal signal = null)
        {
            if (signal is RollExecuteSignal esignal) {
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