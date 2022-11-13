using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	using UnityEditor;
	public class AddAllAssetsPhase : AssetInitializationPhase
	{
		public override List<BuildAsset> GetAssets()
		{
			List<BuildAsset> rs = new List<BuildAsset>();
			foreach (var item in AssetDatabase.GetAllAssetBundleNames())
			{
				
			}
			return rs;
		}
	}
}