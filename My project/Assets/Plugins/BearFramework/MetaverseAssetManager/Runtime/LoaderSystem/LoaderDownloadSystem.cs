using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public static class LoaderDownloadSystem 
	{
		public static string GetDownloadDataPath(this AssetLoader loader,string file){
			return loader.downloader.GetDownloadDataPath(file);
			
		}
		
		public static string GetDownloadURL(this AssetLoader loader,string file){
			return loader.downloader.GetDownloadURL(file);
		}
		
		public static Download DownloadAsync(this AssetLoader loader,string url, string savePath, Action<Download> completed = null,
			long size = 0, string hash = null)
		{
			return loader.downloader.DownloadAsync(url,savePath, completed);
		}
		
		public static Download DownloadAsync(this AssetLoader loader,DownloadInfo info, Action<Download> completed = null)
		{
			return loader.downloader.DownloadAsync(info,completed);
		}
	}
}
