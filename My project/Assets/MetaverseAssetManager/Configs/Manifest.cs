﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
	using System;
	using System.IO;
	public class ManifestData{
		public string bundleExtension;
		public int version;
		public List<string> dirs = new List<string>();
		public List<ManifestAsset> assets = new List<ManifestAsset>();
		public List<ManifestBundle> bundles = new List<ManifestBundle>();
	}
	
	public class Manifest
	{
		public ManifestData data;
		private Dictionary<string, string> aliasWithAssets = new Dictionary<string, string>();
		private Dictionary<string, List<int>> directoryWithAssets = new Dictionary<string, List<int>>();
		private Dictionary<string, ManifestAsset> nameWithAssets = new Dictionary<string, ManifestAsset>();
		private Dictionary<string, ManifestBundle> nameWithBundles = new Dictionary<string, ManifestBundle>();
		public static Func<string, string> customLoader { get; set; }
		public string nameWithAppendHash { get; internal set; }

		public string[] GetAssets()
		{
			var list = new List<string>();
			var assets = data.assets;
			var dirs = data.dirs;
			
			foreach (var asset in assets)
			{
				var path = $"{dirs[asset.dir]}/{asset.name}";
				list.Add(path);
			}

			return list.ToArray();
		}
		public bool Contains(string assetPath)
		{
			return nameWithBundles.ContainsKey(assetPath);
		}

		public ManifestBundle GetBundle(string assetPath)
		{
			return nameWithBundles.TryGetValue(assetPath, out var manifestBundle) ? manifestBundle : null;
		}

		public ManifestBundle[] GetDependencies(ManifestBundle bundle)
		{
			return bundle == null
				? Array.Empty<ManifestBundle>()
				: Array.ConvertAll(bundle.deps, input => data.bundles[input]);
		}
		public void Copy(Manifest manifest)
		{
			data = manifest.data;
			nameWithBundles = manifest.nameWithBundles;
			directoryWithAssets = manifest.directoryWithAssets;
			aliasWithAssets = manifest.aliasWithAssets;
			nameWithAppendHash = manifest.nameWithAppendHash;
			nameWithAssets = manifest.nameWithAssets;
		}
		
		public Manifest(ManifestData data)
		{
			Init(data);
		}
		
		public void Init(ManifestData data){
			nameWithBundles.Clear();
			aliasWithAssets.Clear();
			nameWithAssets.Clear();
			
			var dirs = data.dirs;
			var bundles = data.bundles;
			var assets = data.assets;
			var bundleExtension = data.bundleExtension;
			//create dirs
			foreach (var item in dirs)
			{
				var dir = item;
				if (!directoryWithAssets.TryGetValue(dir, out _)) directoryWithAssets.Add(dir, new List<int>());

				int pos;
				while ((pos = dir.LastIndexOf('/')) != -1)
				{
					dir = dir.Substring(0, pos);
					if (!directoryWithAssets.TryGetValue(dir, out _)) directoryWithAssets.Add(dir, new List<int>());
				}
			}
			
			foreach (var bundle in bundles)
			{
				nameWithBundles[bundle.name] = bundle;
				if (string.IsNullOrEmpty(bundleExtension))
				{
					bundle.nameWithAppendHash = $"{bundle.name}_{bundle.hash}";
				}
				else
				{
					var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(bundle.name);
					bundle.nameWithAppendHash = $"{fileNameWithoutExtension}_{bundle.hash}{bundleExtension}";
				}
			}
			
			foreach (var asset in assets)
			{
				var dir = dirs[asset.dir];
				var path = $"{dir}/{asset.name}";
				asset.path = path;
				AddAsset(path, bundles[asset.bundle]);
				nameWithAssets[path] = asset;
				if (directoryWithAssets.TryGetValue(dir, out var value)) value.Add(asset.id);
			}
		}

		public bool IsDirectory(string path)
		{
			return directoryWithAssets.ContainsKey(path);
		}

		public string[] GetAssetsWithDirectory(string dir, bool recursion)
		{
			var assets = data.assets;
			
			if (!recursion)
				return directoryWithAssets.TryGetValue(dir, out var value)
				? value.ConvertAll(i => assets[i].path).ToArray()
				: Array.Empty<string>();

			var keys = new List<string>();
			foreach (var item in directoryWithAssets.Keys)
				if (item.StartsWith(dir)
				&& (item.Length == dir.Length || (item.Length > dir.Length && item[dir.Length] == '/')))
					keys.Add(item);

			if (keys.Count <= 0) return Array.Empty<string>();

			var get = new List<string>();
			foreach (var item in keys) get.AddRange(GetAssetsWithDirectory(item, false));

			return get.ToArray();
		}
		
		public void SaveAssetWithDirectory(string path)
		{
			var assets = data.assets;
			var filename = Path.GetFileName(path);
			var asset = new ManifestAsset {name = filename, id = assets.Count, path = path };
			assets.Add(asset); 
			var dir = Path.GetDirectoryName(path)?.Replace('\\', '/');
			if (!directoryWithAssets.TryGetValue(dir, out var list))
			{
				list = new List<int>();
				directoryWithAssets.Add(dir, list);
				int pos;
				while ((pos = dir.LastIndexOf('/')) != -1)
				{
					dir = dir.Substring(0, pos);
					if (!directoryWithAssets.TryGetValue(dir, out _))
					{
						directoryWithAssets.Add(dir, new List<int>());
					}
				}
			}
			list.Add(asset.id);
			AddAsset(path, null);
		}
		
		public void AddAsset(string asset, ManifestBundle bundle)
		{
			var newPath = customLoader?.Invoke(asset);
			if (!string.IsNullOrEmpty(newPath))
			{
				if (aliasWithAssets.TryGetValue(newPath, out var assetPath))
				{
					if (!assetPath.Equals(asset)) Logger.W($"{asset} already exist {assetPath}");
				}
				else
				{
					aliasWithAssets[newPath] = asset;
				}
			}

			nameWithBundles[asset] = bundle;
		}
		public bool GetActualPath(string assetName, out string actualPath)
		{
			return aliasWithAssets.TryGetValue(assetName, out actualPath);
		}
	}
	
	
}