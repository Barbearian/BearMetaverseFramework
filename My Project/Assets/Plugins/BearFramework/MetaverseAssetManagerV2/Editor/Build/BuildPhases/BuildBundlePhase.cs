using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Bear.Asset.Editor{
	public class BuildBundlePhase : IBuildPhase
	{
		public void Start(BuildProduct product){
			if (product.assets.Count > 0)
				BuildBundle(product);
		}
		
		
		private void BuildBundle(BuildProduct product){
			var name2bundle = product.bundles.ToDictionary(o=>o.name);
			foreach (var asset in product.assets)
			{
				
				//Create bundle if it doesn't exist
				if(!name2bundle.TryGetValue(asset.bundle,out var bundle)){
					var id = product.bundles.Count;
					bundle = new BuildBundle{
						id = id,
						name = asset.bundle,
						assets = new List<string>()
					};
					
					product.bundles.Add(bundle);
					name2bundle[asset.bundle] = bundle;
				}	
				
				//Added new path
				bundle.assets.Add(asset.path);
			}
			
		
			//Create asset bundle build with bundles
			AssetBundleBuild[] builds = product.bundles.ConvertAll(
				bundle => new AssetBundleBuild{
					assetNames = bundle.assets.ToArray(),
					assetBundleName = bundle.name
				}
			).ToArray();
			
			//Build Asset Bundle
			var manifest = BuildPipeline.BuildAssetBundles(
				product.PlatformCachePath.FolderPath,
				builds,
				product.BuildOptions,
				EditorUserBuildSettings.activeBuildTarget
			);
			
			//Handle null case
			if(manifest == null){
				return;
			}
			
			//initialize bundle
			var bundleNames = manifest.GetAllAssetBundles();
			foreach (var bundleName in bundleNames)
			{
				if(name2bundle.TryGetValue(bundleName,out var bundle)){
					
					var cachePath = product.PlatformCachePath.GetFilePath(bundleName);
					
					bundle.deps = Array.ConvertAll(
						manifest.GetAllDependencies(bundleName),
						name => name2bundle[name].id
					);
					
					var info = new FileInfo(cachePath);
					
					if(info.Exists){
						
						var name = bundleName.Replace(BuildUtils.BundleExtension, string.Empty);
						var hash = Utility.ComputeHash(cachePath);
						var nameWithAppendHash = $"{name}_{hash}{BuildUtils.BundleExtension}";
						var buildPath = product.PlatformCachePath.GetFilePath(nameWithAppendHash);
						
						bundle.nameWithAppendHash = nameWithAppendHash;
					}
				}
			}
		}
	}
}