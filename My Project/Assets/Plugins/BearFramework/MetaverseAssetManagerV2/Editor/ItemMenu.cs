

namespace Bear.Asset.Editor{
	using System.IO;
	using UnityEditor;
	using UnityEngine;
	public static class BuildAssetMenuItems 
	{
		
		[MenuItem("BearFramework/Asset/Build Bundles", false, 100)]
		public static void BuildBundles()
		{
			BuildFactory.Build();
		}
		
		[MenuItem("BearFramework/Asset/Clear Bundles", false, 200)]
		public static void ClearBundles()
		{
			var directory = BuildSettings.PlatformDataPath;
			if (Directory.Exists(directory))
				Directory.Delete(directory, true);
		}
		
		[MenuItem("BearFramework/Asset/Clear Download", false, 200)]
		public static void ClearDownload()
		{
			var directory = Application.persistentDataPath;
			if (Directory.Exists(directory))
				Directory.Delete(directory, true);
		}
	}
}