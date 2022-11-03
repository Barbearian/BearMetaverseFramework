using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class LocalAvatarSignalSender : MonoBehaviour
	{
		public AvatarGenerator receiver;
		[SerializeField]
		public List<ResourceMakeSignal> signals;
		public void Inject(){
			for (int i = 0; i < signals.Count; i++) {
				//	Debug.Log(signals[i].id+": -> "+signals[i].resourceName[0]);
				receiver.ReceiveSignal(signals[i]);
			}
		}
	}
}