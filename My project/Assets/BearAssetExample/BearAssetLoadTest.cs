using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear{
	public class BearAssetLoadTest : MonoBehaviour
	{
		public AssetLoader loader;
		public string manifestAddress;
		public string targetAsset;
	    // Start is called before the first frame update
	    void Start()
	    {
		    Manifest manifest = Manifest.LoadFromFile(manifestAddress);
			loader = new AssetLoader(manifest);
		    loader.LoadAssetAsync<GameObject>(targetAsset,(x)=>{
		    	Debug.Log("hello");
			    Instantiate(x.asset);
		    });
	    }
	    
		public void Update(){
			loader.Update();
		}
	
	}
}
