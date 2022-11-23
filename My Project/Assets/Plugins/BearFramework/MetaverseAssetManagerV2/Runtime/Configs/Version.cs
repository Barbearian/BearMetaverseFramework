using System;


namespace Bear.Asset{
	using UnityEngine;
	[Serializable]
	public class Version: ScriptableObject, ISerializationCallbackReceiver
	{
		public const string Filename = "version.json";
		public long timestamp;

		public string hash;
		public ulong size;
		public string file;
		public string build;
		
		public void OnBeforeSerialize()
		{
			
		}

		public void OnAfterDeserialize()
		{

		}

	}
	

}
