using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class Versions :  ScriptableObject, ISerializationCallbackReceiver
	{
		public const string Filename = "versions.json";
		public long timestamp;
		public List<Version> data = new List<Version>();
		private Dictionary<string, Version> _data = new Dictionary<string, Version>();

		public void OnBeforeSerialize()
		{

		}

		public void OnAfterDeserialize()
		{

		}
		
		public bool TryGetVersion(string build, out Version version)
		{
			return _data.TryGetValue(build, out version);
		}
		
		public bool TryGetAsset(string path, out ManifestAsset asset){
			foreach(var item in data){
				if (!item.manifest.TryGetAsset(path, out var value)) continue;
				asset = value;
				return true;
			}
			
			
			asset = null;
			return false;
		}
	}
}