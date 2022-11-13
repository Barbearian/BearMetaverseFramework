using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Bear{
	public class Asset : Loadable,IEnumerator
	{
		public Action<Asset> completed;
		public Object asset { get; protected set; }
		public Object[] subAssets { get; protected set; }
		public Type type { get; set; }
		public bool isSubAssets { get; set; }
		public Task<Asset> Task
		{
			get
			{
				var tcs = new TaskCompletionSource<Asset>();
				completed += operation => { tcs.SetResult(this); };
				return tcs.Task;
			}
		}
		public bool MoveNext()
		{
			return !isDone;
		}

		public void Reset()
		{
		}
		public object Current => null;
		protected void OnLoaded(Object target)
		{
			asset = target;
			Finish(asset == null ? "asset == null" : null);
		}

		public T Get<T>() where T : Object
		{
			return asset as T;
		}

		protected override void OnComplete()
		{
			if (completed == null) return;

			var saved = completed;
			completed?.Invoke(this);

			completed -= saved;
		}
		protected override void OnUnused()
		{
			completed = null;
		}

		protected override void OnUnload()
		{
			Loader.AssetCache.Remove(pathOrURL);
		}
	}
}