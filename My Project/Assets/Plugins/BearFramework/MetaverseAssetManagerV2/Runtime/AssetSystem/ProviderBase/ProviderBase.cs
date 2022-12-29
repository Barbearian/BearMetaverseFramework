using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bear.Asset{

	internal abstract class ProviderBase{
		public enum EStatus
		{
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
		
		///// <summary>
		///// 所属资源系统
		///// </summary>
		//public AssetSystemImpl Impl { private set; get; }

		///// <summary>
		///// 资源信息
		///// </summary>
		//public AssetInfo MainAssetInfo { private set; get; }
		
		/// <summary>
		/// 获取的资源对象
		/// </summary>
		public UnityEngine.Object AssetObject { protected set; get; }

		/// <summary>
		/// 获取的资源对象集合
		/// </summary>
		public UnityEngine.Object[] AllAssetObjects { protected set; get; }
		
		/// <summary>
		/// 获取的场景对象
		/// </summary>
		public UnityEngine.SceneManagement.Scene SceneObject { protected set; get; }

		/// <summary>
		/// 原生文件路径
		/// </summary>
		public string RawFilePath { protected set; get; }


		/// <summary>
		/// 当前的加载状态
		/// </summary>
		public EStatus Status { protected set; get; } = EStatus.None;

		/// <summary>
		/// 最近的错误信息
		/// </summary>
		public string LastError { protected set; get; } = string.Empty;
		
		/// <summary>
		/// 引用计数
		/// </summary>
		public int RefCount { private set; get; } = 0;

		/// <summary>
		/// 是否已经销毁
		/// </summary>
		public bool IsDestroyed { private set; get; } = false;

		/// <summary>
		/// 是否完毕（成功或失败）
		/// </summary>
		public bool IsDone
		{
			get
			{
				return Status == EStatus.Succeed || Status == EStatus.Failed;
			}
		}
		
		/// <summary>
		/// 加载进度
		/// </summary>
		public virtual float Progress
		{
			get
			{
				return 0;
			}
		}


		protected bool IsWaitForAsyncComplete { private set; get; } = false;
		private readonly List<OperationHandleBase> _handles = new List<OperationHandleBase>();

		//public ProviderBase(AssetSystemImpl impl, string providerGUID, AssetInfo assetInfo)
		//{
		//	Impl = impl;
		//	ProviderGUID = providerGUID;
		//	MainAssetInfo = assetInfo;
		//}

		/// <summary>
		/// 轮询更新方法
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// 销毁资源对象
		/// </summary>
		public virtual void Destroy()
		{
			IsDestroyed = true;
		}
		
		/// <summary>
		/// 是否可以销毁
		/// </summary>
		public bool CanDestroy()
		{
			if (IsDone == false)
				return false;

			return RefCount <= 0;
		}
		
		/// <summary>
		/// 释放操作句柄
		/// </summary>
		public void ReleaseHandle(OperationHandleBase handle)
		{
			if (RefCount <= 0)
				Logger.W("Asset provider reference count is already zero. There may be resource leaks !");

			if (_handles.Remove(handle) == false)
				throw new System.Exception("Should never get here !");

			// 引用计数减少
			RefCount--;
		}

		/// <summary>
		/// 等待异步执行完毕
		/// </summary>
		public void WaitForAsyncComplete()
		{
			IsWaitForAsyncComplete = true;

			// 注意：主动轮询更新完成同步加载
			Update();

			// 验证结果
			if (IsDone == false)
			{
				//Logger.W($"WaitForAsyncComplete failed to loading : {MainAssetInfo.AssetPath}");
			}
		}

		/// <summary>
		/// 异步操作任务
		/// </summary>
		public Task Task
		{
			get
			{
				if (_taskCompletionSource == null)
				{
					_taskCompletionSource = new TaskCompletionSource<object>();
					if (IsDone)
						_taskCompletionSource.SetResult(null);
				}
				return _taskCompletionSource.Task;
			}
		}
		
		#region 异步编程相关
		private TaskCompletionSource<object> _taskCompletionSource;
		protected void InvokeCompletion()
		{
			// 注意：创建临时列表是为了防止外部逻辑在回调函数内创建或者释放资源句柄。
			List<OperationHandleBase> tempers = new List<OperationHandleBase>(_handles);
			foreach (var hande in tempers)
			{
				if (hande.IsValidWithWarning)
				{
					hande.InvokeCallback();
				}
			}

			if (_taskCompletionSource != null)
				_taskCompletionSource.TrySetResult(null);
		}
		#endregion
	}
}