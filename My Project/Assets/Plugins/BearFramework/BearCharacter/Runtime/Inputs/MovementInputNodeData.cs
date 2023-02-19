using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class MovementInputNodeData : INodeData, IGlobalNodeDataAccessor
    {
        private bool requsted;
        public CameraNodeData data;
        public System.Action<Vector3> forward;
        public Vector3 dir;
        public MovementInputNodeData()
        {
            this.RequestGlobalNodeData<CameraNodeData>(NodeDataRegistered);
        }
        public void Move(Vector2 inputDir)
        {
            if (requsted) {
                dir = data.GetInputDir(inputDir);
                forward?.Invoke(dir);
            }
        }
        private void NodeDataRegistered(CameraNodeData data)
        {
            requsted = true;
            this.data = data;
            
            
        }

    }

    public interface IMovementInputReceiver{
        System.Action<Vector3> DOnReceiveMovementInput{get;}
    }


}