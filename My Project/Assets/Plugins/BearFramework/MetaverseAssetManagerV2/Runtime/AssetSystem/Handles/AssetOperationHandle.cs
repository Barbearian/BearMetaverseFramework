using UnityEngine;
using System.Collections.Generic;

namespace Bear.Asset
{
	public sealed class AssetOperationHandle : OperationHandleBase
	{
		private System.Action<AssetOperationHandle> _callback;
		internal AssetOperationHandle(ProviderBase provider) : base(provider)
		{
		}
		internal override void InvokeCallback()
		{
			_callback?.Invoke(this);
		}
		/// <summary>
		/// 完成委托
		/// </summary>
		public event System.Action<AssetOperationHandle> Completed
		{
			add
			{
				if (IsValidWithWarning == false)
					throw new System.Exception($"{nameof(AssetOperationHandle)} is invalid");
				if (Provider.IsDone)
					value.Invoke(this);
				else
					_callback += value;
			}
			remove
			{
				if (IsValidWithWarning == false)
					throw new System.Exception($"{nameof(AssetOperationHandle)} is invalid");
				_callback -= value;
			}
		}
		
		/// <summary>
		/// 资源对象
		/// </summary>
		public UnityEngine.Object AssetObject
		{
			get
			{
				if (IsValidWithWarning == false)
					return null;
				return Provider.AssetObject;
			}
		}

		/// <summary>
		/// 获取资源对象
		/// </summary>
		/// <typeparam name="TAsset">资源类型</typeparam>
		public TAsset GetAssetObject<TAsset>() where TAsset : UnityEngine.Object
		{
			if (IsValidWithWarning == false)
				return null;
			return Provider.AssetObject as TAsset;
		}
		
		/// <summary>
		/// 等待异步执行完毕
		/// </summary>
		public void WaitForAsyncComplete()
		{
			if (IsValidWithWarning == false)
				return;
			Provider.WaitForAsyncComplete();
		}
		
		/// <summary>
		/// 释放资源句柄
		/// </summary>
		public void Release()
		{
			this.ReleaseInternal();
		}


	}
}
