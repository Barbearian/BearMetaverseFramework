using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bear{
	public class AsyncOperationSystem
	{
		private static readonly List<AsyncOperationBase> _operations = new List<AsyncOperationBase>(100);
		// Timer related
		private static Stopwatch _watch;
		private static long _frameTime;
		
		/// <summary>
		/// max time slice
		/// </summary>
		public static long MaxTimeSlice { set; get; } = long.MaxValue;
		
		/// <summary>
		/// Whether operation is busy
		/// </summary>
		public static bool IsBusy
		{
			get
			{
				return _watch.ElapsedMilliseconds - _frameTime >= MaxTimeSlice;
			}
		}
		
		/// <summary>
		/// Initialize Async Operation System
		/// </summary>
		public static void Initialize()
		{
			_watch = Stopwatch.StartNew();
		}
		
		/// <summary>
		/// Update async operation system 
		/// </summary>
		public static void Update()
		{
			_frameTime = _watch.ElapsedMilliseconds;

			for (int i = _operations.Count - 1; i >= 0; i--)
			{
				if (IsBusy)
					return;

				var operation = _operations[i];
				operation.Update();
				if (operation.IsDone)
				{
					_operations.RemoveAt(i);
					operation.Finish();
				}
			}
		}
		
		/// <summary>
		/// Destory Async Operation
		/// </summary>
		public static void DestroyAll()
		{
			_operations.Clear();
			_watch = null;
			_frameTime = 0;
			MaxTimeSlice = long.MaxValue;
		}
		
		/// <summary>
		/// Start an Async Operation
		/// </summary>
		public static void StartOperation(AsyncOperationBase operationBase)
		{
			_operations.Add(operationBase);
			operationBase.Start();
		}
	}
}