using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	using System;
	using System.IO;
	public class Manifest : ScriptableObject, ISerializationCallbackReceiver
	{
		public string build; // the name of manifest
		public string[] dirs = Array.Empty<string>();
		public ManifestAsset[] assets = Array.Empty<ManifestAsset>();
		public ManifestBundle[] bundles = Array.Empty<ManifestBundle>();
		private readonly Dictionary<string,List<int>> directory2assets = new Dictionary<string, List<int>>();
		private readonly Dictionary<string,ManifestAsset> name2assets = new Dictionary<string, ManifestAsset>();
		
		
		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			//Get all directories
			foreach (var item in dirs)
			{
				var dir = item;
				if(!directory2assets.TryGetValue(dir,out _)) directory2assets.Add(dir,new List<int>());
				
				int pos;
				while((pos = dir.LastIndexOf("/"))!=-1){
					dir = dir.Substring(0,pos);
					if(!directory2assets.TryGetValue(dir,out _)) directory2assets.Add(dir,new List<int>());
				}
			}
			
			//Change bundle's name with appended has
			foreach (var bundle in bundles)
			{
				var extension = Path.GetExtension(bundle.name);
				var nameWithAppendHash = string.IsNullOrEmpty(extension)
					?$"{bundle.name}_{bundle.hash}"
					:$"{bundle.name.Replace(extension, string.Empty)}_{bundle.hash}{extension}";
				bundle.nameWithAppendHash = nameWithAppendHash;

			}
			
			foreach (var asset in assets)
			{
				var dir = dirs[asset.dir];
				var path = $"{dir}/{asset.name}";
				
				asset.path= path;
				AddAsset(asset);
			
			}
			
		}
		
		private void AddAsset(ManifestAsset asset){
			name2assets[asset.path] = asset;
			
			//
		}
	}
}