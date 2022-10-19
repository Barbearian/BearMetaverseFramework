using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public static class LoaderPathSystem
	{
		/// <summary>
		///     平台名字，主要用在网络下载和编辑器
		/// </summary>
		public static string PlatformName { get; set; } = Utility.GetPlatformName();
		
		public static string GetActualPath(this AssetLoader loader,string asset)
		{
			if (loader.manifest.GetActualPath(asset, out var value)) 
				return value;
			else{
				return asset;
			}
		}
		
		public static string GetBundlePathOrURL(this AssetLoader loader,ManifestBundle bundle){
			var assetBundleName = bundle.nameWithAppendHash;
			if (loader.BundleWithPathOrUrLs.TryGetValue(assetBundleName, out var path)) return path;

			var containsKey = loader.StreamingAssets.Contains(assetBundleName);
			if(LoaderVersionSystem.OfflineMode || containsKey){
				path = loader.GetPlayerDataPath(assetBundleName);
				loader.BundleWithPathOrUrLs[assetBundleName] = path;
				return path;
				
			}
			
			if(loader.IsDownloaded(bundle)){
				path = loader.GetDownloadDataPath(assetBundleName);
				loader.BundleWithPathOrUrLs[assetBundleName] = path;
				return path;

			}
			
			path = loader.GetDownloadURL(assetBundleName);
			loader.BundleWithPathOrUrLs[assetBundleName] = path;
			return path;
		}

	}
}