using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public static class LoaderUpdateSystem 
	{
		public static void Update(this AssetLoader loader){
			loader.LoadableUpdater.Update();
			//loader.downloader.Update();
		}
	}
}