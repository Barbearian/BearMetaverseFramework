using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public interface IUpdatable
	{
		void Update();
		void Clear();
	}

	public static class UpdatableSystem {
		public static float _realtimeSinceUpdateStartup;
		public static float maxUpdateTimeSlice = 0.01f;
		public static bool Busy() { 
			return Time.realtimeSinceStartup - _realtimeSinceUpdateStartup >= maxUpdateTimeSlice;
		}



	}
}