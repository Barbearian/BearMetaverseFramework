using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class Versions 
	{
		public Versions(Manifest manifest){
			Manifest = manifest;
			ReloadPlayerVersions(null);
		}
		
		public Versions(Manifest manifest,BuildVersions versions){
			Manifest = manifest;
			ReloadPlayerVersions(versions);
		}
		
		public const string Filename = "versions.json";
		public const string APIVersion = "2022.1p4";
		public readonly Manifest Manifest;

		public readonly List<string> StreamingAssets = new List<string>();

		public static VerifyMode VerifyMode { get; set; } = VerifyMode.Hash;

		/// <summary>
		///     是否是仿真模式
		/// </summary>
		public bool SimulationMode { get;  set; }

		/// <summary>
		///     是否是离线模式
		/// </summary>
		public bool OfflineMode { get; set; }

		/// <summary>
		///     本地版本的时间戳
		/// </summary>
		public long Timestamp { get;  set; }

		/// <summary>
		///     获取清单的版本号
		/// </summary>
		public string ManifestVersion => Manifest.data.version.ToString();

		/// <summary>
		///     加载安装包的版本文件。
		/// </summary>
		/// <param name="versions">安装包的版本文件</param>
		public void ReloadPlayerVersions(BuildVersions versions)
		{
			StreamingAssets.Clear();
			// 版本数据为空的时候，是仿真模式。
			if (versions == null)
			{
				SimulationMode = true;
				OfflineMode = true;
				return;
			}

			Timestamp = versions.timestamp;
			StreamingAssets.AddRange(versions.streamingAssets);
			OfflineMode = versions.offlineMode;
			SimulationMode = false;
		}
		
		
		
	}
}