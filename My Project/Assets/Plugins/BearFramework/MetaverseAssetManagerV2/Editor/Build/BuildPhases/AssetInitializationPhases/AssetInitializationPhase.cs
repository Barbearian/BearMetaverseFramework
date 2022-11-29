using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public abstract class AssetInitializationPhase : IBuildPhase
	{
		public void Start(BuildProduct product){
			product.assets = GetAssets();
		}
		public abstract List<BuildAsset> GetAssets();
	}
}