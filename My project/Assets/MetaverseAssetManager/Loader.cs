using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AssetLoader 
	{
		LoadableUpdater LoadableUpdater = new LoadableUpdater();
		public List<Loadable> Loading => LoadableUpdater.Loading;
		public List<Loadable> Unused => LoadableUpdater.Unused;	
	}
}
