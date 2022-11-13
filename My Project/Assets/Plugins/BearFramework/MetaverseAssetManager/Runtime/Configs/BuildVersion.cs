using System;

namespace Bear{
	[Serializable]
	public class BuildVersion
	{
		public string name;
		public string file;
		public long size;
		public string hash;
	}
}