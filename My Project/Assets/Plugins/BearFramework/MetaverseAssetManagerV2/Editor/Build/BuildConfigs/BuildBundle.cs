using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	using System;
	public class BuildBundle
	{
		public int id;
		public string name;
		public int[] deps = Array.Empty<int>();
		public List<string> assets = new List<string>();
	}
}