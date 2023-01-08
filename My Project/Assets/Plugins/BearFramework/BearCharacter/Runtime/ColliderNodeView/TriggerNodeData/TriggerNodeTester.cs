using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class TriggerNodeTester : MonoBehaviour
	{
		public void Start(){
			gameObject.AddComponent<NodeView>().AddNodeData<DestoryTriggerNodeData>(new DestoryTriggerNodeData());
			
		}
	}
}