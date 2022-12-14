using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bear.editor{
	using System.IO;
	public class CreateManifest : BuildTaskJob
	{
		public CreateManifest(BuildTask task) : base(task)
		{
		}
		
		public override void Run(){
			var forceRebuild = _task.forceRebuild;
			var buildNumber = _task.buildVersion;
			var versions = BuildVersions.Load(GetBuildPath(Versions.Filename));
			//	Debug.Log(GetBuildPath(Versions.Filename));
			var version = versions.Get(_task.name);
			//Debug.Log(GetBuildPath(version?.file));

			var manifest = Manifest.LoadFromFile(GetBuildPath(version?.file));
			if (buildNumber > 0)
				manifest.data.version = buildNumber;
			else
				manifest.data.version++;
				
			_task.buildVersion = manifest.data.version;
			var getBundles = new Dictionary<string, ManifestBundle>();
			foreach (var bundle in manifest.data.bundles) getBundles[bundle.name] = bundle;
			
			var dirs = new List<string>();
			var assets = new List<ManifestAsset>();
			var manifestAssets = new Dictionary<string, ManifestAsset>();
			var bundles = _task.bundles;

			for (var index = 0; index < bundles.Count; index++)
			{
				var bundle = bundles[index];
				foreach (var asset in bundle.assets)
				{
					var dir = Path.GetDirectoryName(asset)?.Replace("\\", "/");
					var pos = dirs.IndexOf(dir);
					if (pos == -1)
					{
						pos = dirs.Count;
						dirs.Add(dir);
					}

					var manifestAsset = new ManifestAsset
					{
						name = Path.GetFileName(asset),
						bundle = index,
						dir = pos,
						id = assets.Count
                    };
					assets.Add(manifestAsset);
					manifestAssets.Add(asset, manifestAsset);
				}
				
				if (getBundles.TryGetValue(bundle.name, out var value) && value.hash == bundle.hash) continue;

				changes.Add(bundle.nameWithAppendHash);

			}
			
			if (changes.Count == 0 && !forceRebuild)
			{
				error = "Nothing to build.";
				Debug.LogWarning(error);
				return;
			}
			
			GetDependencies(manifestAssets);
			manifest.data.bundles = bundles;
			manifest.data.assets = assets;
			manifest.data.dirs = dirs;
			manifest.data.bundleExtension = Settings.BundleExtension;
			_task.changes.AddRange(changes);
			_task.SaveManifest(manifest);


		}
		
		private static void GetDependencies(Dictionary<string, ManifestAsset> assets)
		{
			foreach (var pair in assets)
			{
				var asset = pair.Value;
				var deps = new HashSet<int>();
				var dependencies = Settings.GetDependencies(pair.Key);
				foreach (var dependency in dependencies)
					if (assets.TryGetValue(dependency, out var value))
						deps.Add(value.id);

				asset.deps = deps.ToArray();
			}
		}
	}
}