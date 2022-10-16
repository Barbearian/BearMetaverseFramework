using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System.IO;
	public interface ILoadble:IUpdatable{
		public AssetLoader Loader{get;set;}
	}
	public class Loadable:ILoadble
	{        
		public AssetLoader Loader{get;set;}
		public readonly Reference _reference = new Reference();
		public LoadableStatus status { get; protected set; } = LoadableStatus.Wait;
		public string pathOrURL { get; protected set; }
		public string error { get; internal set; }		

		public bool isDone => status == LoadableStatus.SuccessToLoad || status == LoadableStatus.Unloaded ||
		status == LoadableStatus.FailedToLoad;
		
		public float progress { get; protected set; }

		protected void Finish(string errorCode = null)
		{
			error = errorCode;
			status = string.IsNullOrEmpty(errorCode) ? LoadableStatus.SuccessToLoad : LoadableStatus.FailedToLoad;
			progress = 1;
		}
		
		public void Update()
		{
			OnUpdate();
		}

		public void Complete()
		{
			if (status == LoadableStatus.FailedToLoad)
			{
				Logger.E("Unable to load {0} {1} with error: {2}", GetType().Name, pathOrURL, error);
				Release();
			}

			OnComplete();
		}
		protected virtual void OnUpdate()
		{
		}

		protected virtual void OnLoad()
		{
		}

		protected virtual void OnUnload()
		{
		}

		protected virtual void OnComplete()
		{
		}

		protected void Load()
		{
			
			var Unused = Loader.Unused;
			var Loading = Loader.Loading;
			if (status != LoadableStatus.Wait && _reference.unused) Unused.Remove(this);

			_reference.Retain();
			Loading.Add(this);
			if (status != LoadableStatus.Wait) return;
			Logger.I("Load {0} {1}.", GetType().Name, pathOrURL);
			status = LoadableStatus.Loading;
			progress = 0;
			OnLoad();
		}
		
		public void Unload()
		{
			if (status == LoadableStatus.Unloaded) return;
			Logger.I("Unload {0} {1}.", GetType().Name, pathOrURL, error);
			OnUnload();
			status = LoadableStatus.Unloaded;
		}
		
		public void Release()
		{
			var Unused = Loader.Unused;

			if (_reference.count <= 0)
			{
				Logger.W("Release {0} {1}.", GetType().Name, Path.GetFileName(pathOrURL));
				return;
			}

			_reference.Release();
			if (!_reference.unused) return;

			Unused.Add(this);
			OnUnused();
		}
		
		protected virtual void OnUnused()
		{
		}
	}
	public enum LoadableStatus
	{
		Wait,
		Loading,
		DependentLoading,
		SuccessToLoad,
		FailedToLoad,
		Unloaded
	}
}