using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System.IO;
	public enum VerifyMode
	{
		Size,
		Hash
	}

	public static class LoaderVersionSystem
	{
		public static VerifyMode VerifyMode { get; set; } = VerifyMode.Hash;

		
		public static bool ContainsAsset(this AssetLoader Loader,string assetPath)
		{
			return Loader.manifest.Contains(assetPath);
		}
		
		public static bool GetDependencies(this AssetLoader Loader,string assetPath, out ManifestBundle mainBundle, out ManifestBundle[] dependencies){
			if (Loader.manifest.Contains(assetPath))
			{
				mainBundle = Loader.manifest.GetBundle(assetPath);
				dependencies = Loader.manifest.GetDependencies(mainBundle);
				return true;
			}
			
			mainBundle = null;
			dependencies = null;
			return false;
		}
		/// <summary>
		///     判断 bundle 是否已经下载
		/// </summary>
		/// <param name="bundle"></param>
		/// <param name="checkStreamingAssets"></param>
		/// <returns></returns>
		public static bool IsDownloaded(this AssetLoader loader,ManifestBundle bundle, bool checkStreamingAssets = true)
		{
			if (bundle == null) return false;

			if (loader.OfflineMode || (checkStreamingAssets && loader.StreamingAssets.Contains(bundle.nameWithAppendHash))) return true;

			var path = loader.GetDownloadDataPath(bundle.nameWithAppendHash);
			var file = new FileInfo(path);
			if (!file.Exists) return false;

			if (file.Length == bundle.size && VerifyMode == VerifyMode.Size) return true;

			if (file.Length < bundle.size) return false;

			return bundle.hash == Utility.ComputeHash(path);
		}
	}
}