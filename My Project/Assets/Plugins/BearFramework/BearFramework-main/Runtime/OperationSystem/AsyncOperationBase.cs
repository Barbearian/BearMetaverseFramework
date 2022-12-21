using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{

	
	using System.Threading.Tasks;
	using System;
	public interface IUpdator{
		public void Update();
	}
	
	public abstract class AsyncOperationBase : IEnumerator,IUpdator
	{
		// Requested Callback
		private Action<AsyncOperationBase> _callback;

		/// <summary>
		/// Status
		/// </summary>
		public EOperationStatus Status { get; protected set; } = EOperationStatus.None;

		/// <summary>
		/// Error Message
		/// </summary>
		public string Error { get; protected set; }

		/// <summary>
		/// Progress
		/// </summary>
		public float Progress { get; protected set; }
		/// <summary>
		/// Is Done
		/// </summary>
		public bool IsDone
		{
			get
			{
				return Status == EOperationStatus.Failed || Status == EOperationStatus.Succeed;
			}
		}
		/// <summary>
		/// CompleteEvent
		/// </summary>
		public event Action<AsyncOperationBase> Completed
		{
			add
			{
				if (IsDone)
					value.Invoke(this);
				else
					_callback += value;
			}
			remove
			{
				_callback -= value;
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
		
		public abstract void Start();
		public abstract void Update();
		internal void Finish()
		{
			Progress = 1f;
			_callback?.Invoke(this);
			if (_taskCompletionSource != null)
				_taskCompletionSource.TrySetResult(null);
		}
		
		/// <summary>
		/// 清空完成回调
		/// </summary>
		protected void ClearCompletedCallback()
		{
			_callback = null;
		}
		
		#region 异步编程相关
		bool IEnumerator.MoveNext()
		{
			return !IsDone;
		}
		void IEnumerator.Reset()
		{
		}
		object IEnumerator.Current => null;

		private TaskCompletionSource<object> _taskCompletionSource;
		#endregion
	}
}