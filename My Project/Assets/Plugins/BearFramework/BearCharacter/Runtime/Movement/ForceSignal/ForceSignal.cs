using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AddForceSignal : INodeSignal
	{
		public Vector3 force;
	}
	
	public class StopForceSignal: INodeSignal{
		
	}
}
