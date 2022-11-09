using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	[Serializable]
	public sealed class BundleRequest : LoadRequest
	{
		internal IBundleRequestHandler handler;
		internal AssetBundle assetBundle { get; set; }
		public ManifestBundle info { get; set; }
		protected override void OnStart()
		{
			handler.OnStart();
		}
		
		protected override void OnUpdated()
		{
			handler.Update();
		}

		protected override void OnWaitForCompletion()
		{
			handler.WaitForCompletion();
		}	
		
		public void LoadAssetBundle(string filename, ulong offset = 0){
			Logger.D($"Load {info.nameWithAppendHash} from {filename} with offset {offset}");
			loader.ReloadAssetBundle(info.name);
			assetBundle = AssetBundle.LoadFromFile(filename,0,offset);
			progress = 1;
			if (assetBundle == null)
			{
				SetResult(Result.Failed, $"assetBundle == null, {info.nameWithAppendHash}");
				return;
			}

			SetResult(Result.Success);
			loader.AddAssetBundle(info.name, assetBundle);
		}
		
		protected override void OnDispose(){
			loader.RemoveBundleRequest(this);
			handler.Dispose();
			if(assetBundle != null){
				assetBundle.Unload(true);
				loader.RemoveAssetBundle(info.name);
				assetBundle = null;
			}
			
			Reset();
		}
		
		
	}
}