using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bear{
	public abstract class AsyncOperationBase : IEnumerator
	{
		
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
		/// Progress on operation
		/// </summary>
		public float Progress { get; protected set; }
		
		
		/// <summary>
		/// Is operation finished
		/// </summary>
		public bool IsDone
		{
			get
			{
				return Status == EOperationStatus.Failed || Status == EOperationStatus.Succeed;
			}
		}
		
		/// <summary>
		/// Completed event
		/// </summary>
		public event Action<AsyncOperationBase> Completed
		{
			add
			{
				if(IsDone)
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
		/// Async Operation Task
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
		
		internal abstract void Start();
		internal abstract void Update();
		internal void Finish()
		{
			Progress = 1f;
			_callback?.Invoke(this);
			if (_taskCompletionSource != null)
				_taskCompletionSource.TrySetResult(null);
		}
		
		/// <summary>
		/// Clear complete event
		/// </summary>
		protected void ClearCompletedCallback()
		{
			_callback = null;
		}
		
		#region AsyncOperation
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
	
	public enum EOperationStatus{
		None,
		Succeed,
		Failed
	}
}