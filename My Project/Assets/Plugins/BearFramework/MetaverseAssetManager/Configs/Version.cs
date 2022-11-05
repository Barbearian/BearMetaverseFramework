using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	[Serializable]
	public class Version
	{
		public string build;
		public string file;
		public ulong size;
		public string hash;
		public int ver;
		public Manifest manifest { get; set; }
	}
}