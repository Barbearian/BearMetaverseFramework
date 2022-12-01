using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public abstract class ProviderBase
	{
		public enum EStatus{
			None = 0,
			CheckBundle,
			Loading,
			Checking,
			Succeed,
			Failed,
		}
		
		/// <summary>
		/// 资源提供者唯一标识符
		/// </summary>
		public string ProviderGUID { private set; get; }
	}
}