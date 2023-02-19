using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
namespace Bear
{
    public class CharacterDirectionAdjustNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        private NodeView view;
        private MovementInputNodeData movementData;
        public void Attached(INode node)
        {
            movementData = node.GetOrCreateNodeData<MovementInputNodeData>();

            if (node is NodeView myView) { 
                view= myView;

                node.RegisterSignalReceiver<FaceInputDirSignal>((x) => {
                    var dir = movementData.dir;
                    if (dir.sqrMagnitude == 0)
                    {
                        dir = myView.transform.forward;
                    }

                    var force = movementData.dir * x.moveAmount;

                    node.ReceiveNodeSignal(new UpdateFacingSignal()
                    {
                        direction = dir
                    });

                    node.ReceiveNodeSignal(new AddForceSignal()
                    {
                        force = force
                    });
                }, true).AddTo(receivers);
            }
           
        }
    }

    public struct FaceInputDirSignal:INodeSignal {
        public float moveAmount;
    }
}