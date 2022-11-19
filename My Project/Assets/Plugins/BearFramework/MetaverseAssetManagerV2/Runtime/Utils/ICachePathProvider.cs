using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public interface IFolderPathProvider
	{
		public string FolderPath{get;}
	}
	
	public static class IFolderPathProviderSystem{
		public static string GetFilePath(this IFolderPathProvider provider,string filename){
			return $"{provider.FolderPath}/{filename}";
		}
	}
	
	public class FolderPathProvider:IFolderPathProvider{
		public string _folderPath;
		public string FolderPath =>_folderPath;
	}
}