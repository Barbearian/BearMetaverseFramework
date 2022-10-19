using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public static class BundleLoaderSystem 
	{
		public static Bundle LoadBundleInternally(this AssetLoader loader,ManifestBundle bundle){
			if (bundle == null) throw new NullReferenceException();
			if(!loader.BundleCache.TryGetValue(bundle.nameWithAppendHash, out var item)){
				var url = loader.GetBundlePathOrURL(bundle);
				if(loader.customBundleLoader != null) item = loader.customBundleLoader(url,bundle);
				
				if(item == null){
					if (url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("ftp://"))
						item = new DownloadBundle {pathOrURL = url, info = bundle};
					else
						item = new LocalBundle {pathOrURL = url, info = bundle};
				}
				
				loader.BundleCache.Add(bundle.nameWithAppendHash, item);
			}
	
			item.Load();
			return item;
		}
	}
}