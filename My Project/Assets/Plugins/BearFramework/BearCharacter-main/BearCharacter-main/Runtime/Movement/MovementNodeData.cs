using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [System.Serializable]
    public class MovementNodeData : INodeData
    {
	    public float speedMulti = 1;
        public bool isMoving;
        public Vector3 dir;

    }

    public struct MovementOutputNodeData :INodeData{
        public System.Action<Vector3> DMove;
    }
    
	public static class MovementKeyword{
		public const string StopMoving = "StopMoving";
	}
}