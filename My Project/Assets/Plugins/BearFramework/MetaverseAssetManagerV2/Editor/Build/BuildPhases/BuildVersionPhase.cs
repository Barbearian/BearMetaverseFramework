using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear.Asset.Editor{
	using System.IO;
	using System.Text;
	public class BuildVersionPhase : IBuildPhase
	{
		List<ManifestAsset> assets = new List<ManifestAsset>();
		List<string> dirs = new List<string>();
		Dictionary<string,ManifestBundle> name2bundle = new Dictionary<string, ManifestBundle>();
		
		public void Start(BuildProduct product){

			Init();
			
			var version = Utility.LoadFromFile<Version>(product.DataPath.GetFilePath(Version.Filename));
			var manifest =  Utility.LoadFromFile<Manifest>(product.DataPath.GetFilePath(version.file));

			if(!BuildManifest(product,manifest)) return;
			
			BuildVersion(product,version,manifest);
			
		}
		
		private void Init(){
			List<ManifestAsset> assets = new List<ManifestAsset>();
			dirs = new List<string>();
			name2bundle = new Dictionary<string, ManifestBundle>();
			name2bundle = new Dictionary<string, ManifestBundle>();

		}
		
		private bool BuildManifest(BuildProduct product, Manifest manifest){
			if(!CheckChanges(product,manifest)) return false;
			
			if(BuildManifestAssets(product,manifest))
				if(BuildManifestBundles(product,manifest)){
					manifest.build = product.build;
					return true;
				}
					
			return false;
		}
		
		private bool CheckChanges(BuildProduct product, Manifest manifest){
			
			foreach (var bundle in manifest.bundles)
			{
				name2bundle[bundle.name] = bundle; 
			}
			
			for (int i = 0; i < product.bundles.Count; i++) {
				var bundle = product.bundles[i];

				// check bundle update
				if(name2bundle.TryGetValue(bundle.name,out var mBundle) && 
					bundle.hash == mBundle.hash &&
					bundle.size == mBundle.size
				) continue;
				
				product.changes.Add(bundle.name);
			}
			if(product.changes.Count == 0 && !product.ForceRebuild && product.bundles.Count == name2bundle.Count){
				return false;
			}
			return true;
			
		}
		
		private bool BuildManifestAssets(BuildProduct product, Manifest manifest){
			for (int i = 0; i < product.bundles.Count; i++) {
				var bundle = product.bundles[i];
				foreach (var asset in bundle.assets)
				{
					var mAsset = GetManifestAsset(asset,i);
					assets.Add(mAsset);
				}
				
			}
			
			manifest.assets = assets.ToArray();
			return true;
		}
		
		private ManifestAsset GetManifestAsset(string assetPath,int bundleIndex){
			var dir = Path.GetDirectoryName(assetPath)?.Replace("\\","/");
			var pos = dirs.IndexOf(dir);
			if(pos == -1){
				pos = dirs.Count;
				dirs.Add(dir);
			}
			
			var id = assets.Count;
				
			var mAsset = new ManifestAsset(){
				name = Path.GetFileName(assetPath),
				id = id,
				dir = pos,
				bundle = bundleIndex
			};
			
			return mAsset;
		}
		
		private bool BuildManifestBundles(BuildProduct product, Manifest manifest){
			var bundles = product.bundles.ConvertAll(
				bbundle => new ManifestBundle()
				{
					name = bbundle.name,
					size = bbundle.size,
					hash = bbundle.hash,
					deps = bbundle.deps,
					
				}
			);
			
			manifest.bundles = bundles.ToArray();
			return true;
		}
		
		private void BuildVersion(BuildProduct product, Version version, Manifest manifest){
			
			
			
			var json = JsonUtility.ToJson(manifest);
			var bytes = Encoding.UTF8.GetBytes(json);
			var hash = Utility.ComputeHash(bytes);
			var buildToLower = manifest.build.ToLower();
			var file =  $"{buildToLower}_{hash}.json";
			var manifestPath = product.DataPath.GetFilePath(file);
			//write manifest
			File.WriteAllText(manifestPath, json);
			product.changes.Add(file);
			
			//Write version
			var info = new FileInfo(manifestPath);
			var size = (ulong) info.Length;
			
			version.file = file;
			version.size = size;
			version.hash = hash;
			version.build = product.build;

			
			File.WriteAllText(
				product.DataPath.GetFilePath(Version.Filename), 
				JsonUtility.ToJson(version));

		}
	}
}