using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.editor{
	using UnityEditor;
	public static class MenuItems
	{
		[MenuItem("BearFramework/Asset/Build Bundles", false, 10)]
		public static void BuildBundles()
		{
			BuildScript.BuildBundles();
		}
		
		[MenuItem("BearFramework/Asset/Copy Build to StreamingAssets ", false, 50)]
		public static void CopyBuildToStreamingAssets()
		{
			BuildScript.CopyToStreamingAssets();
		}
		
		[MenuItem("BearFramework/Asset/Clear Build", false, 800)]
		public static void ClearBuild()
		{
			BuildScript.ClearBuild();
		}
	}
}