using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public static class AssetLoaderDependenciesSystem
	{
		private static readonly Dictionary<string, Dependencies> Loaded = new Dictionary<string, Dependencies>();

		
		public static Dependencies LoadDependenciesAsync(this AssetLoader loader,ManifestAsset asset)
		{
			if (!Loaded.TryGetValue(asset.path, out var value))
			{
				value = new Dependencies(loader){_asset = asset};
				Loaded[asset.path] = value;
			}

			
			value.LoadAsync();
			return value;
		}
	}
}