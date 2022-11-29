using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	using System;
	[System.Serializable]
	public class ManifestBundle
	{
		public int[] deps = Array.Empty<int>();
		public string hash;
		public string name;
		public ulong size;
		public string nameWithAppendHash { get; set; }
	}
}