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
		
		/// <summary>
		///     设置 bundle 的加载地址缓存
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <param name="url"></param>
		internal static void SetBundlePathOrURl(this AssetLoader loader,string assetBundleName, string url)
		{
			loader.BundleWithPathOrUrLs[assetBundleName] = url;
		}
		
		public static string GetBundlePathOrURL(this AssetLoader loader,ManifestBundle bundle){
			var assetBundleName = bundle.nameWithAppendHash;
			if (loader.BundleWithPathOrUrLs.TryGetValue(assetBundleName, out var path)) return path;

			var containsKey = loader.StreamingAssets.Contains(assetBundleName);
			if(loader.OfflineMode|| containsKey){
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
		
		public static string GetTemporaryPath(this AssetLoader loader,string file){
			var path = $"{Application.temporaryCachePath}/{file}";
			Utility.CreateDirectoryIfNecessary(path);
			return path;

		}

	}
}