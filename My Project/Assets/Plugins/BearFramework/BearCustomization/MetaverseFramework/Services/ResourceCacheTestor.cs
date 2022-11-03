using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class ResourceCacheTestor : MonoBehaviour
	{
		public GameObject pref;
		public string key;
		
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			ResourceCache.Set(pref.name,pref);
			var obj = ResourceCache.Get(key);
			Instantiate(obj);
		}
		
		[ContextMenu("TestPath")]
		public void TestPath(){
			Debug.Log(string.Join(",", CacheUtil.GetPath(key)));
		}
	}
}