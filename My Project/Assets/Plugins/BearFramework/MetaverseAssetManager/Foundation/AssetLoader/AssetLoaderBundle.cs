using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public static class AssetLoaderBundleSystem
	{
		#region Hotreload

		private static readonly Dictionary<string, AssetBundle> AssetBundles = new Dictionary<string, AssetBundle>();
		public static void AddAssetBundle(this AssetLoader loader,string name, AssetBundle assetBundle)
		{
			AssetBundles[name] = assetBundle;
		}
		
		public static void ReloadAssetBundle(this AssetLoader loader,string name){
			if(!AssetBundles.TryGetValue(name,out var assetBundle)) return;
			if (assetBundle != null) assetBundle.Unload(false);
			AssetBundles.Remove(name);

		}
		
		public static void RemoveAssetBundle(this AssetLoader loader,string name)
		{
			AssetBundles.Remove(name);
		}
		#endregion

		#region Internal
		
		private static readonly Queue<BundleRequest> Unused = new Queue<BundleRequest>();
		public static readonly Dictionary<string, BundleRequest> Loaded = new Dictionary<string, BundleRequest>();
		
		private static IBundleRequestHandler GetHandler(BundleRequest request){

			
			var loader =request.loader;
			
			if (loader.IsWebGLPlatform)
				throw new NotImplementedException("开源版不支持 WebGL");
			var bundle = request.info;

			if (loader.IsPlayerAsset(bundle.hash))
				return new LocalBundleRequestHandler {path = loader.GetPlayerDataPath(bundle.nameWithAppendHash), request = request};

			if (loader.IsDownloaded(bundle))
				return new LocalBundleRequestHandler {path = loader.GetDownloadDataPath(bundle.nameWithAppendHash), request = request};

			throw new NotImplementedException("Download bundle is not implemented yet");

		}
		
		public static void RemoveBundleRequest(this AssetLoader loader,BundleRequest request){
			Loaded.Remove(request.info.nameWithAppendHash);
			Unused.Enqueue(request);
		}
		
		public static BundleRequest LoadBundleRequest(this AssetLoader loader,ManifestBundle bundle){
			if(!Loaded.TryGetValue(bundle.nameWithAppendHash,out var request)){
				request = Unused.Count > 0 ? Unused.Dequeue():new BundleRequest();
				request.loader = loader;
				request.Reset();
				request.info = bundle;
				request.path = bundle.nameWithAppendHash;
				request.handler = GetHandler(request);
				Loaded[bundle.nameWithAppendHash] = request;

			}
			
			
			request.LoadAsync();
			return request;
		}

		#endregion
	}
}