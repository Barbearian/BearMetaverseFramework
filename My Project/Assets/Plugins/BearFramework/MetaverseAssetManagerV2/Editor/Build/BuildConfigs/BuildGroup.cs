using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public class BuildGroup : ScriptableObject
	{
		public Object[] entries;
		public BundleMode bundleMode = BundleMode.PackByFile;
	}
	
	public static class AssetGroupSystem{
		public static BuildAsset[] GetBuildAssets(this BuildGroup group){
			var assets = new List<BuildAsset>();
			if (group.entries == null) return assets.ToArray();

			return assets.ToArray();
		}
	}
}