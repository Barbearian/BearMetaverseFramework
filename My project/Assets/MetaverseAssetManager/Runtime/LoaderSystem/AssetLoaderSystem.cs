using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;

	public static class AssetLoaderSystem
	{
		internal static Asset CreateAssetInstance(this AssetLoader loader,string path, Type type)
		{
			if (string.IsNullOrEmpty(path)) throw new ArgumentException(nameof(path));

			return loader.AssetCreator(path, type);
		}
		
		public static Asset LoadAssetAsync(this AssetLoader loader,string path, Type type, Action<Asset> completed = null)
		{
			return loader.LoadAssetInternal(path, type, completed);
		}

		public static Asset LoadAsset(this AssetLoader loader,string path, Type type)
		{
			Asset asset = loader.LoadAssetInternal(path, type);
			asset.LoadImmediate();
			return asset;
		}
		public static Asset LoadWithSubAssets(this AssetLoader loader,string path, Type type)
		{
			Asset asset = loader.LoadAssetInternal(path, type);
			if (asset == null)
				return null;
			asset.isSubAssets = true;
			asset.LoadImmediate();
			return asset;
		}

		public static Asset LoadWithSubAssetsAsync(this AssetLoader loader,string path, Type type)
		{
			Asset asset = loader.LoadAssetInternal(path, type);
			if (asset == null)
				return null;
			asset.isSubAssets = true;
			return asset;
		}
		internal static Asset LoadAssetInternal(this AssetLoader loader,string path, Type type,
			Action<Asset> completed = null)
		{
			path = loader.GetActualPath(path);
			if (!loader.ContainsAsset(path))
			{
				Logger.E("FileNotFoundException {0}", path);
				return null;
			}

			if (!loader.AssetCache.TryGetValue(path, out var item))
			{
				item = loader.CreateAssetInstance(path, type);
				loader.AssetCache.Add(path, item);
			}

			if (completed != null) item.completed += completed;

			item.Load();
			return item;
		}
	}
}