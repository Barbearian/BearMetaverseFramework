using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System.IO;
	public static class AssetLoaderDownload
	{
		public static bool IsDownloaded(this AssetLoader loader,Version version){
			var path = loader.GetDownloadDataPath(version.file);
			var file = new FileInfo(path);
			if (!file.Exists || file.Length != (long) version.size) return false;
			if (loader.FastVerifyMode) return true;
			return Utility.ComputeHash(path) == version.hash;

		}
		
		public static bool IsDownloaded(this AssetLoader loader, ManifestBundle bundle){
			if (loader.IsPlayerAsset(bundle.hash)) return true;
			var path = loader.GetDownloadDataPath(bundle.nameWithAppendHash);
			var file = new FileInfo(path);
			if (!file.Exists || file.Length != (long) bundle.size) return false;
			if (loader.FastVerifyMode) return true;
			return Utility.ComputeHash(path) == bundle.hash;
		}
		
		public static bool IsPlayerAsset(this AssetLoader loader,string key){
			return loader.PlayerAssets!=null && loader.PlayerAssets.Contains(key);
		}
		
		
	}
}