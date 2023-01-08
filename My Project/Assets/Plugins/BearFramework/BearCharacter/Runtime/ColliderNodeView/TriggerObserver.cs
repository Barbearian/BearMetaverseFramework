using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear{
	using System;
	[RequireComponent(typeof(Collider))]
	public class TriggerObserver : MonoBehaviour
	{
		public Action<Collider> DOTriggerEnter;
		public Action<Collider> DOTriggerExit;
		public void OnTriggerEnter(Collider collider){
			DOTriggerEnter?.Invoke(collider);
		}
		
		public void OnTriggerExit(Collider collider){
			DOTriggerExit?.Invoke(collider);

		}
	}
}