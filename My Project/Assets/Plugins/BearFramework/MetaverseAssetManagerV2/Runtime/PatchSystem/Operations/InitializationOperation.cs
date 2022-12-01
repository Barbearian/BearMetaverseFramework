using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	/// <summary>
	/// 初始化操作
	/// </summary>
	public abstract class InitializationOperation : AsyncOperationBase
	{
		/// <summary>
		/// 初始化内部加载的包裹版本
		/// </summary>
		public string InitializedPackageVersion;
	}
}