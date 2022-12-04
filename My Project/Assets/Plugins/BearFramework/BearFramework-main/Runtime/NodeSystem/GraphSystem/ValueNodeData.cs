using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class TagNodeData : INodeData
	{
		public List<string> tags = new List<string>();
	}
	
	public class PropertyNodeData: INodeData{
		public Dictionary<string,string> properties = new Dictionary<string, string>();
	}
}