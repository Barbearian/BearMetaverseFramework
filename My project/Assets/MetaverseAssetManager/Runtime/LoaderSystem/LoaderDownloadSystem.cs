using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public static class LoaderDownloadSystem 
	{
		public static string GetDownloadDataPath(this AssetLoader loader,string file){
			return loader.downloader.GetDownloadDataPath(file);
			
		}
		
		public static string GetDownloadURL(this AssetLoader loader,string file){
			return loader.downloader.GetDownloadURL(file);
		}
	}
}
