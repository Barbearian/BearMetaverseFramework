using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	using System;
	public class Manifest : ScriptableObject, ISerializationCallbackReceiver
	{
		public string build;
		public string[] dirs = Array.Empty<string>();
		public ManifestAsset[] assets = Array.Empty<ManifestAsset>();
		public ManifestBundle[] bundles = Array.Empty<ManifestBundle>();
		
		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{

		}
	}
}