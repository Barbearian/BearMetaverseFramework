using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear.Asset{
	internal class BundleInfo 
	{
		public enum ELoadMode
		{
			None,
			LoadFromStreaming,
			LoadFromCache,
			LoadFromRemote,
			LoadFromEditor,
		}
		
		public readonly ManifestBundle Bundle;


		//public readonly PatchBundle Bundle;
		public readonly ELoadMode LoadMode;
		
		
		/// <summary>
		/// Remote Download Address
		/// </summary>
		public string[] RemoteURL { private set; get; } = new string[0];
		
		/// <summary>
		/// Editor Asset Path
		/// </summary>
		public string EditorAssetPath { private set; get; }
		
		private BundleInfo()
		{
		}
		
		public BundleInfo(ManifestBundle patchBundle, ELoadMode loadMode, params string[] url)
		{
			Bundle = patchBundle;
			LoadMode = loadMode;
			RemoteURL = url;
			EditorAssetPath = string.Empty;
		}
		
		public BundleInfo(ManifestBundle patchBundle, ELoadMode loadMode, string editorAssetPath)
		{
			Bundle = patchBundle;
			LoadMode = loadMode;
			EditorAssetPath = editorAssetPath;
		}
		public BundleInfo(ManifestBundle patchBundle, ELoadMode loadMode)
		{
			Bundle = patchBundle;
			LoadMode = loadMode;
			EditorAssetPath = string.Empty;
		}
	}
}