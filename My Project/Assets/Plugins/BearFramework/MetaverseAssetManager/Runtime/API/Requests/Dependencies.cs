using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class Dependencies
	{
		public Dependencies(AssetLoader loader){
			this.loader = loader;
		}
		
		private AssetLoader loader;
		
		private readonly List<BundleRequest> _bundles = new List<BundleRequest>();
		private readonly List<BundleRequest> _loading = new List<BundleRequest>();
		public ManifestAsset _asset;
		private BundleRequest _bundleRequest;
		private int _refCount;
		

		public AssetBundle assetBundle => _bundleRequest.assetBundle;
		public bool isDone => _loading.Count == 0;
		public float progress => (_bundles.Count - _loading.Count) * 1f / _bundles.Count;

		private BundleRequest Load(ManifestBundle bundle){
			var request = loader.LoadBundleRequest(bundle);
			_bundles.Add(request);
			_loading.Add(request);
			return request;
		}
		
		public void LoadAsync(){
			if (_refCount == 0)
			{
				var bundles = _asset.manifest.bundles;
				var bundle = bundles[_asset.bundle];
				_bundleRequest = Load(bundle);
				foreach (var dep in bundle.deps)
					Load(bundles[dep]);
			}

			_refCount++;
		}
		
		public bool CheckResult(LoadRequest request)
		{
			foreach (var bundle in _bundles)
			{
				if (bundle.result != Request.Result.Failed) continue;
				request.SetResult(Request.Result.Failed, bundle.error);
				return false;
			}

			return true;
		}
		
		public void WaitForCompletion()
		{
			for (var index = 0; index < _loading.Count; index++)
			{
				var request = _loading[index];
				request.WaitForCompletion();
				_loading.RemoveAt(index);
				index--;
			}
		}
		
		public void Release()
		{
			_refCount--;
			if (_refCount != 0) return;

			foreach (var request in _bundles) request.Release();

			_bundles.Clear();
		}
		
		public void Update(){
			if (isDone) return;
			for (var index = 0; index < _loading.Count; index++)
			{
				var request = _loading[index];
				if (!request.isDone) continue;
				_loading.RemoveAt(index);
				index--;
			}
		}
	}
}