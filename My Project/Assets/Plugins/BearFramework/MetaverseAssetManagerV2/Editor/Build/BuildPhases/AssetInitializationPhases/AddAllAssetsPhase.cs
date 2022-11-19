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
			foreach (var bundle in AssetDatabase.GetAllAssetBundleNames())
			{
				foreach (string asset in AssetDatabase.GetAssetPathsFromAssetBundle(bundle))
				{
					rs.Add(
						new BuildAsset(){
							path = asset,
							bundle = bundle,
						}
					);
				}
			}
			return rs;
		}
	}
}