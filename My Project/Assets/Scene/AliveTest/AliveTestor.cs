using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear{
	public class AliveTestor : MonoBehaviour
	{
		public string id = "test1";
		public AlivePoolNodeView pool;
		public void Update(){
			pool.Ping(id,DOnEnd);
		}
		
		private void DOnEnd(){
			Debug.Log(id+" is dead");
		}
	}
}