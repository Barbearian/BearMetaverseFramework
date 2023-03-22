using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bear{
	[RequireComponent(typeof(TriggerObserver))]
	public class TriggerObserverTestor : MonoBehaviour
	{
		public UnityEvent DOnEnter;
		public void Awake(){
			this.GetComponent<TriggerObserver>().DOTriggerEnter += (x)=>{ DOnEnter.Invoke(); };
		}

		public void Log(string message)
		{
			Debug.Log(message);

		}
	}
}