using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public class AssetInitializationPhase
	{
		public void Start(BuildProduct product){
			foreach (var group in product.groups)
			{
				if(group == null){
					//
					continue;
				}
				
				
			}
		}
		

	}
	

}