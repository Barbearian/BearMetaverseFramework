using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	public abstract class OperationHandleBase :IEnumerator
	{
		internal ProviderBase Provider { private set; get; }
		internal abstract void InvokeCallback();

		internal OperationHandleBase(ProviderBase provider)
		{
			Provider = provider;
			//_assetInfo = provider.MainAssetInfo;
		}
		#region 异步操作相关

		
		
		/// <summary>
		/// 句柄是否有效
		/// </summary>
		internal bool IsValidWithWarning
		{
			get
			{
				if (Provider != null && Provider.IsDestroyed == false)
				{
					return true;
				}
				else
				{
					//if (Provider == null)
					//	Logger.W($"Operation handle is released : {_assetInfo.AssetPath}");
					//else if (Provider.IsDestroyed)
					//	Logger.W($"Provider is destroyed : {_assetInfo.AssetPath}");
					return false;
				}
			}
		}
		
		/// <summary>
		/// 是否加载完毕
		/// </summary>
		public bool IsDone
		{
			get
			{
				if (IsValidWithWarning == false)
					return false;
				return Provider.IsDone;
			}
		}
		
		/// <summary>
		/// 释放句柄
		/// </summary>
		internal void ReleaseInternal()
		{
			if (IsValidWithWarning == false)
				return;
			Provider.ReleaseHandle(this);
			Provider = null;
		}
		
		/// <summary>
		/// 异步操作任务
		/// </summary>
		public System.Threading.Tasks.Task Task
		{
			get { return Provider.Task; }
		}

		// 协程相关
		bool IEnumerator.MoveNext()
		{
			return !IsDone;
		}
		void IEnumerator.Reset()
		{
		}
		object IEnumerator.Current
		{
			get { return Provider; }
		}
		#endregion
	}
	
	public enum EOperationStatus
	{
		None,
		Succeed,
		Failed
	}
}