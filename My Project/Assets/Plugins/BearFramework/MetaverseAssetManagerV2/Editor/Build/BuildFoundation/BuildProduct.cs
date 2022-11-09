using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public class BuildProduct
	{
		//inputs
		public List<BuildGroup> groups = new List<BuildGroup>();
		
		//outputs
		public List<BuildBundle> bundles = new List<BuildBundle>();
		public List<BuildAsset> assets = new List<BuildAsset>();
		public List<string> changes = new List<string>();
	}
}