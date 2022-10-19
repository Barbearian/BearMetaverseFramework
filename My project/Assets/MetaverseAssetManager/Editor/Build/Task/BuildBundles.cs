using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Bear.editor{
	public class BuildBundles : BuildTaskJob
	{
		private readonly BuildAssetBundleOptions _options;
		public readonly List<ManifestBundle> bundles = new List<ManifestBundle>();
	
		public BuildBundles(BuildTask task, BuildAssetBundleOptions options) : base(task)
		{
			_options = options;
		}
		
		public override void Run(){
			
		}

	}
}