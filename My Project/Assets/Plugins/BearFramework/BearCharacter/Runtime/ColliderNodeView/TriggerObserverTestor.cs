using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	[RequireComponent(typeof(TriggerObserver))]
	public class TriggerObserverTestor : MonoBehaviour
	{
		public string message;
		public void Awake(){
			this.GetComponent<TriggerObserver>().DOTriggerEnter += (x)=>{Debug.Log(message);};
		}
	}
}