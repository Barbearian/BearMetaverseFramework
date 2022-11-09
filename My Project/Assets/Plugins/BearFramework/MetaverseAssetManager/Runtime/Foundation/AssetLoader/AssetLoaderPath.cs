using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public static class AssetLoaderPath
	{
		public static string GetPlayerDataPath(this AssetLoader Loader,string filename){
			return $"{Loader.DownloadURL}/{filename}";
		}
		
		public static string GetDownloadDataPath(this AssetLoader Loader,string filename){
			var path = $"{Loader.DownloadDataPath}";
			Utility.CreateDirectoryIfNecessary(path);
			return path;
		}
		
		public static string GetPlayerDataURl(this AssetLoader Loader,string filename)
		{
			return $"{Loader.Protocol}{Loader.GetPlayerDataPath(filename)}";
		}
	}
}