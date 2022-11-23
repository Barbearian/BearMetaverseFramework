using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	using UnityEditor;
	public class BuildProduct
	{

		//inputs
		public IFolderPathProvider PlatformCachePath;
		public IFolderPathProvider DataPath;
		public bool ForceRebuild;

		public BuildAssetBundleOptions BuildOptions = BuildAssetBundleOptions.ChunkBasedCompression;
		
		//outputs
		public List<BuildAsset> assets = new List<BuildAsset>();
		public List<BuildBundle> bundles = new List<BuildBundle>();
		
		//changes
		public List<string> changes = new List<string>();
	}
}