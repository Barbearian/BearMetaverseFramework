using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System.IO;
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
		public FolderPathProvider(string folderPath,bool CreateDirWhenNonExists = false){
			this._folderPath = folderPath;
			if(CreateDirWhenNonExists){
				Directory.CreateDirectory(folderPath);
			}
			
		}
		
		public string _folderPath;
		public string FolderPath =>_folderPath;
	}
}