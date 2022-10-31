using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear{
	public class BearAssetLoadTest : MonoBehaviour
	{
		public AssetLoader loader;
		public DownloadUpdater dupdater = new DownloadUpdater();
		public string DownloadAddress;
		public string manifestAddress;
		public string targetAsset;
	    // Start is called before the first frame update
	    void Start()
	    {
		    Manifest manifest = Manifest.LoadFromFile(manifestAddress);
			Versions version = new Versions(manifest);
			version.OfflineMode = false;
			Downloader downloader = new Downloader(DownloadAddress);

			loader = new AssetLoader(version,downloader);
			
		    loader.LoadAssetAsync<GameObject>(targetAsset,(x)=>{
		    	Debug.Log("hello");
			    Instantiate(x.asset);
		    });
	    }
	    
		public void Update(){
			UpdatableSystem._realtimeSinceUpdateStartup = Time.realtimeSinceStartup;

			loader.Update();
			dupdater.Update();
		}

        private void OnDestroy()
        {
			DownloadSystem.ClearAllDownloads();
        }

    }
}
