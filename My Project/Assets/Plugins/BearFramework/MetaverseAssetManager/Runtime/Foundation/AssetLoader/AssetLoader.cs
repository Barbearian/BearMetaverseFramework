using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AssetLoader
	{
		public string UniqueIdenitifier = "localhost";
		public const string Bundles = "Bundles";
		public string DownloadURL{get;set;}
		public Versions Versions {get;set;} = ScriptableObject.CreateInstance<Versions>();
		public PlayerAssets PlayerAssets { get; set; } = ScriptableObject.CreateInstance<PlayerAssets>();
		public bool SimulationMode { get; set; }
		public bool FastVerifyMode { get; set; } = true;
		public Platform Platform { get; set; } = Utility.GetPlatform();
		public bool IsWebGLPlatform => Platform == Platform.WebGL;
		public string Protocol => Utility.GetProtocol();
		public string PlayerDataPath => $"{Application.streamingAssetsPath}/{UniqueIdenitifier}/{Bundles}";
		public string DownloadDataPath {get;set;}

		public AssetLoader(){
			DownloadDataPath=  $"{Application.persistentDataPath}/{UniqueIdenitifier}/{Bundles}";
		}
	}
}