using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AnimationStateListenerBehavior : StateMachineBehaviour
	{
		public List<AnimationMoment> moments;
	}
	
	[System.Serializable]
	public class AnimationMoment{
		[Range(0f,1f)]
		public float moment;
		public string key;
	}
}