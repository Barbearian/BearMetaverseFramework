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
		
		/// <summary>
		///     获取下载信息。
		/// </summary>
		/// <param name="file">指定的文件名</param>
		/// <param name="hash">指定的文件哈希</param>
		/// <param name="size">指定文件的下载大小</param>
		/// <param name="fastVerify"></param>
		/// <returns></returns>
		public static DownloadInfo GetDownloadInfo(this AssetLoader loader,string file, string hash, long size, bool fastVerify = true)
		{
			if (Versions.VerifyMode == VerifyMode.Size && fastVerify) hash = null;

			var info = new DownloadInfo
			{
				hash = hash,
				size = size,
				savePath = loader.GetDownloadDataPath(file),
				url = loader.GetDownloadURL(file)
            };
			return info;
		}
		
		public static void LoadVersion(this AssetLoader loader,BuildVersion version){
			var path = loader.GetDownloadDataPath(version.file);
			var manifest = Manifest.LoadFromFile(path);
			manifest.name = version.name;
			Logger.I("LoadVersion:{0} with file {1}.", version.name, path);
			manifest.nameWithAppendHash = version.file;
			loader.LoadVersion(manifest);
			
		}
		
		public static void LoadVersion(this AssetLoader loader,Manifest manifest){
			loader.manifest.Copy(manifest);
			
		}
		
		public static bool Exist(this AssetLoader loader,BuildVersion version){
			if(version == null) return false;
			
			var info = new FileInfo(loader.GetDownloadDataPath(version.file));
			return 
			(
				info.Exists && 
				info.Length == version.size &&
				VerifyMode == VerifyMode.Size
			)		
				||
			Utility.ComputeHash(info.FullName) == version.hash;
			
			
		}
		
		public static bool Changed(AssetLoader loader,BuildVersion version){
			return loader.manifest.nameWithAppendHash != version.file;
		}
		
		public static void ReloadPlayerVersions(this AssetLoader loader,BuildVersions versions){
			loader.ReloadPlayerVersions(versions);

		}

	}
}