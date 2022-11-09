using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	using System;
	[Serializable]
	public class BuildAsset 
	{
		public string path;
		public string bundle;
		public BuildGroup group{get;set;}
	}
}