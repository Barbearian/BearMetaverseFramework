using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class Manifest : ScriptableObject
	{
		public string build;
		public string[] dirs = Array.Empty<string>();
		public ManifestAsset[] assets = Array.Empty<ManifestAsset>();
		public ManifestBundle[] bundles = Array.Empty<ManifestBundle>();
		private readonly Dictionary<string, List<int>> directoryWithAssets = new Dictionary<string, List<int>>();
		private readonly Dictionary<string, ManifestAsset> nameWithAssets = new Dictionary<string, ManifestAsset>();

		public bool TryGetAsset(string path, out ManifestAsset asset)
		{
			return nameWithAssets.TryGetValue(path, out asset);
		}

	}
}