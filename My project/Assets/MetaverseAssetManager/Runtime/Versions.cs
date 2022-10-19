using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class Versions 
	{
		public static Versions myVersions = new Versions();
		public const string Filename = "versions.json";
		public const string APIVersion = "2022.1p4";
		
		public readonly List<string> StreamingAssets = new List<string>();


	}
}