using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	using System;
	public static class BuildFactory
	{
		
		
		public static void Build(){
			var platformCachePath = new FolderPathProvider(BuildSettings.PlatformCachePath,true);
			var dataPath = new FolderPathProvider(BuildSettings.PlatformDataPath,true);
			Build(platformCachePath,dataPath,
				new AddAllAssetsPhase(),
				new BuildBundlePhase(),
				new BuildVersionPhase()
			);
		}
		
		public static void Build(IFolderPathProvider platformCachePath,IFolderPathProvider DataPath,params IBuildPhase[] phases){
			BuildProduct product = new BuildProduct();
			product.PlatformCachePath = platformCachePath;
			product.DataPath = DataPath;
			
			foreach (var phase in phases)
			{
				//try{
					phase.Start(product);
			//	}catch(Exception e){
			//		Logger.E(e.Message);
			//	}
			}
		}
	}
}