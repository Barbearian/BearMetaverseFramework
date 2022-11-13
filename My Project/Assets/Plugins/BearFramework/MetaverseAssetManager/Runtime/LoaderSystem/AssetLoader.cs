using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class AssetLoader 
	{

		
		public AssetLoader(Versions version,Downloader downloader = null){
			this.versions = version;
			if (downloader != null) {
				this.downloader = downloader;
			}
		}
	
		
		#region Loadable
		public LoadableUpdater LoadableUpdater = new LoadableUpdater();
		public List<Loadable> Loading => LoadableUpdater.Loading;
		public List<Loadable> Unused => LoadableUpdater.Unused;	
		#endregion 
		
		#region Asset
		public readonly Dictionary<string, Asset> AssetCache = new Dictionary<string, Asset>();
		public Func<string, Type, Asset> AssetCreator { get; set; } = BundledAsset.Create;

		#endregion
		
		#region pathManager
		public readonly Dictionary<string, string> BundleWithPathOrUrLs = new Dictionary<string, string>();
		public string PlayerDataPath { get; set; } = $"{Application.streamingAssetsPath}/{Utility.buildPath}";

		/// <summary>
		///     获取指定文件相对安装包的加载路径，专供 AssetBundle 加载使用。
		/// </summary>
		/// <param name="file">指定文件的文件名</param>
		/// <returns>指定文件相对安装包的加载路径</returns>
		public string GetPlayerDataPath(string file)
		{
			return $"{PlayerDataPath}/{file}";
		}
		#endregion 
		
		#region version
		
		public Versions versions;
		public List<string> StreamingAssets =>versions.StreamingAssets;
		public bool OfflineMode => versions.OfflineMode;
		public Manifest manifest => versions.Manifest;
		
		#endregion

		#region dependencies
		public readonly Dictionary<string, Dependencies> DependenciesCache = new Dictionary<string, Dependencies>();
		#endregion
		
		#region bundle
		public readonly Dictionary<string, Bundle> BundleCache = new Dictionary<string, Bundle>();
		public Func<string, ManifestBundle, Bundle> customBundleLoader { get; set; } = null;
		#endregion
		
		#region downloader
		/// <summary>
		///     下载数据目录，所有使用 <see cref="GetDownloadURL" /> 下载的文件默认保存在此目录
		/// </summary>
		public Downloader downloader = new Downloader();
		#endregion

		#region operations
		public OperationUpdater operationUpdater = new OperationUpdater();
		public List<Operation> Processing => operationUpdater.Processing;
		#endregion

    }
}
