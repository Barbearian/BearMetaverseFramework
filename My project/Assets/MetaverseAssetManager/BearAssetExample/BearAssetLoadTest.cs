using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class BearAssetLoadTest : MonoBehaviour
	{
		public string manifestAddress;
		public string targetAsset;
	    // Start is called before the first frame update
	    void Start()
	    {
		    Manifest manifest = Manifest.LoadFromFile(manifestAddress);
		    AssetLoader loader = new AssetLoader(manifest);
		    loader.LoadAssetAsync<GameObject>("Sphere.prefab",(x)=>{Debug.Log("hello");});
		    Debug.Log(JsonUtility.ToJson(manifest));
	    }
	
	}
}
