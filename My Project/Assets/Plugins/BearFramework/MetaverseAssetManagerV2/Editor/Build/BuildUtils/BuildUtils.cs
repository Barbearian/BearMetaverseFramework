using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	using UnityEditor;
	public static class BuildUtils
	{
		public static string BundleExtension => ".bundle";
		
		public static T[] FindAssets<T>() where T:ScriptableObject
		{
			var assets = new List<T>();
			var guids = AssetDatabase.FindAssets("t:"+typeof(T).FullName);
			
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				if(string.IsNullOrEmpty(path)) continue;
				
				var asset = AssetDatabase.LoadAssetAtPath<T>(path);
				assets.Add(asset);
			}
			return assets.ToArray();
		}
		
		public static T GetOrCreateAsset<T>(string path) where T:ScriptableObject
		{
			var asset = AssetDatabase.LoadAssetAtPath<T>(path);
			if(asset != null) return asset;
			
			Utility.CreateDirectoryIfNecessary(path);
			asset = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(asset,path);
			return asset;
		}

	}
}