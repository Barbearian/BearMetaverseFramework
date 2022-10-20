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
	}
}