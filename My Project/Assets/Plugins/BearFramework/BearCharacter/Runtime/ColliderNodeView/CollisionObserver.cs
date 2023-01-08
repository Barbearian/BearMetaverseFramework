using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;

	[RequireComponent(typeof(Collider))]
	public class CollisionObserver : MonoBehaviour
	{
		public Action<Collision> DOnCollisionEnter;
		public Action<Collision> DOnCollisionExit;

		public void OnCollisionEnter(Collision collision){
			DOnCollisionEnter?.Invoke(collision);
		}
		
		public void OnCollisionExit(Collision collision){
			DOnCollisionExit?.Invoke(collision);
		}
	}
}