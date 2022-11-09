using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public enum BundleMode{
		PackTogether,
		PackByEntry,
		PackByFile,
		PackByFolder,
		PackByTopSubFolder,
		PackByCustom

	}
	public static class BundleSystem
	{
		public static string GetBundleName(BuildAsset asset){
			return "";
		}
	}
}