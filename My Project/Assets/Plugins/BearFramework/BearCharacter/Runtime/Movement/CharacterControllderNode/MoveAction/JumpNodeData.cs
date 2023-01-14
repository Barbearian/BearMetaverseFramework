using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class JumpNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		public float JumpStrength;
		public NodeView root;

		private ActionNodeSignalReceiver AddForceR;
		public void Attached(INode node){
			if(node is NodeView view){
				root = view;

				AddForceR = root.RegisterSignalReceiver<AddJumpForceSignal>((x) => {
                    root.ReceiveNodeSignal(GetForceSignal());
                });

            }
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
		public static void Jump(this JumpNodeData node){
			//node.root.ReceiveNodeSignal(node.GetForceSignal());
            node.root.ReceiveNodeSignal(new AnimatorClipsPlayerNodeSignal()
            {
                info = new PlayAnimationClipInfo()
                {
                    clipName = "Jump",

                }
            });
        }
	}

	public class AddJumpForceSignal:IAnimationEventSignal { 
	
	}
}