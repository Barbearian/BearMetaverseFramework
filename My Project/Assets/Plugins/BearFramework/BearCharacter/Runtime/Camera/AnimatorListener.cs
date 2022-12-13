using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.Events;
	public class AnimatorListener : MonoBehaviour
	{
		public Animator anim;
		public string AnimatorParameter;
		// Awake is called when the script instance is being loaded.

		public UnityEvent<float> DOnUpdate;
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			DOnUpdate?.Invoke(anim.GetFloat(AnimatorParameter));
		}
		
		
	}
}