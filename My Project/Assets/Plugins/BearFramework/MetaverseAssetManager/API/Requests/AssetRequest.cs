using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class AssetRequest : LoadRequest
	{

		public IAssetRequestHandler handler { get; set; }

		public Object asset { get; set; }
		public Object[] assets { get; set; }
		public bool isAll { get; set; }
		public ManifestAsset info { get; set; }
		public Type type { get; set; }


		protected override void OnStart()
		{
			handler.OnStart();
		}
		
		protected override void OnWaitForCompletion()
		{
			handler.WaitForCompletion();
		}

		protected override void OnUpdated()
		{
			handler.Update();
		}
		
		protected override void OnDispose()
		{
			loader.RemoveAssetRequest(this);
			handler.Dispose();
			asset = null;
			assets = null;
			isAll = false;
		}
		

	}
}