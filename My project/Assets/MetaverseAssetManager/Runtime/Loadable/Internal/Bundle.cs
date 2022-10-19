using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class Bundle : Loadable
	{
		public ManifestBundle info;

		public AssetBundle assetBundle { get; protected set; }
		
		protected AssetBundleCreateRequest LoadAssetBundleAsync(string url)
		{
			Logger.I("LoadAssetBundleAsync", info.nameWithAppendHash);
			return AssetBundle.LoadFromFileAsync(url);
		}
		
		protected AssetBundle LoadAssetBundle(string url)
		{
			Logger.I("LoadAssetBundle", info.nameWithAppendHash);
			return AssetBundle.LoadFromFile(url);
		}
		
		protected void OnLoaded(AssetBundle bundle)
		{
			assetBundle = bundle;
			Finish(assetBundle == null ? "assetBundle == null" : null);
		}
		
		protected override void OnUnload()
		{
			Loader.BundleCache.Remove(info.nameWithAppendHash);
			if (assetBundle == null) return;

			assetBundle.Unload(true);
			assetBundle = null;
		}
	}
}