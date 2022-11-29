using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	[System.Serializable]
	public class ManifestAsset 
	{
		public string name;
		public int bundle; //bundle index in manifest bundle list
		public int id;// self index in manifest asset list
		public int dir; // dir index in manifest dir list
		public string path { get; set; }

	}
}