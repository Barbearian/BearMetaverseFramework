using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear {
	public class AddForceSignal : INodeSignal
	{
		public Vector3 force;
	}

	public class StopForceSignal : INodeSignal {

	}

	public class UpdateMovingSignal : INodeSignal {
		public bool IsMoving;
	}

    public class UpdateRotatingSignal : INodeSignal
    {
        public bool IsRotating;
    }
	public class UpdateRotateSignal : INodeSignal {
		public Quaternion direction;
	}

    public class UpdateFacingSignal : INodeSignal
    {
        public Vector3 direction;
    }
}
