using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class DestoryTriggerNodeData : TriggerNodeData
	{
		public override void OnTriggerEnter(Collider collider)
		{
			//base.OnTriggerEnter(collider);
			GameObject.Destroy(view.gameObject);
		}
	}
}