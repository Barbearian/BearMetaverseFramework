using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	using UnityEditor;
	using System;
	public class BuildSettings : ScriptableObject
	{
		public string downloadURL = "http://127.0.0.1/";
		public static Func<string> GetBundlesPath =>GetBundlePath ;
		public static string PlatformCachePath =>
		$"{Environment.CurrentDirectory}/{GetBundlesPath()}Cache/{Platform}".Replace('\\', '/');

		public static string PlatformDataPath =>
		$"{Environment.CurrentDirectory.Replace('\\', '/')}/{GetBundlesPath()}/{Platform}";
		
		public static Platform Platform => GetPlatform();
		public static string Filename => $"Assets/BearMetaverseFramework/Config/{nameof(BuildSettings)}.asset";
		public static BuildSettings GetDefaultSettings(){
			var settings = BuildUtils.FindAssets<BuildSettings>();
			var setting = 
				settings.Length>0?
				settings[0]:
				BuildUtils.GetOrCreateAsset<BuildSettings>(Filename);
			
			return setting;
		}
		
		private static string GetBundlePath(){
			return "Bundles";
		} 

		private static Platform GetPlatform()
		{
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (EditorUserBuildSettings.activeBuildTarget)
			{
			case BuildTarget.Android:
				return Platform.Android;
			case BuildTarget.StandaloneOSX:
				return Platform.OSX;
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return Platform.Windows;
			case BuildTarget.iOS:
				return Platform.iOS;
			case BuildTarget.WebGL:
				return Platform.WebGL;
			case BuildTarget.StandaloneLinux64:
				return Platform.Linux;
			default:
				return Platform.Default;
			}
		}
	}
}