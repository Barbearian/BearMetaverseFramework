using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public interface IFloatHandler
	{
		public float GetValue(float value);
	}
	
	[System.Serializable]
	public class ClampAndMultiplier:IFloatHandler{
		public float MinValue;
		public float MaxValue;
		public float Multiplier = 1;
		
		
		public float GetValue(float value){
			var target = value*Multiplier;
			target = Mathf.Clamp(target,MinValue,MaxValue);
			return target;
		}
		
		public float GetValueByPercentage(float value){
			
			var target = value*(MaxValue-MinValue) + MinValue;
			return Mathf.Clamp(target,MinValue,MaxValue);
		}
	}
}