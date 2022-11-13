using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public class BuildProduct
	{

		
		//outputs
		public List<BuildAsset> assets = new List<BuildAsset>();
		public List<BuildBundle> bundles = new List<BuildBundle>();
		public List<string> changes = new List<string>();
	}
}