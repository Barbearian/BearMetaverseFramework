using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public static class DependenciesLoaderSystem 
	{
		public static Dependencies LoadDependencies(this AssetLoader loader,string path){
			if (!loader.DependenciesCache.TryGetValue(path, out var item))
			{
				item = new Dependencies {Loader = loader,pathOrURL = path};
				loader.DependenciesCache.Add(path, item);
			}

			item.Load();
			return item;
		}
	}
}