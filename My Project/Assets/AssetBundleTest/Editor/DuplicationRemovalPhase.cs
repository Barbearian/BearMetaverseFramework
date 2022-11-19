using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public class DuplicationRemovalPhase : IBuildPhase
	{
		public void Start(BuildProduct product){
			Dictionary<string,BuildAsset> assets = new Dictionary<string, BuildAsset>();
			for (int i = 0; i < product.assets.Count; i++) {
				var asset = product.assets[i];
				var path = asset.path;
				if(assets.ContainsKey(path)){
					product.assets.RemoveAt(i);
					i--;
				}else{
					assets[path] = asset;
				}
			}
		}
	}
}