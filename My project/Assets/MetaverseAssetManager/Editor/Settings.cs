using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Bear.editor{
	using System.IO;
	using UnityEditor;
	[CreateAssetMenu(menuName = "Bear/MetaverseAsset/Settings", fileName = "Settings", order = 0)]
	public sealed class Settings : ScriptableObject
	{
		
		public static string BundleExtension { get; set; } = ".bundle";

		/// <summary>
		///     采集资源或依赖需要过滤掉的文件
		/// </summary>
		[Header("Bundle")] [Tooltip("采集资源或依赖需要过滤掉的文件")]
		public List<string> excludeFiles =
			new List<string>
			{
			".spriteatlas",
			".giparams",
			"LightingData.asset"
            }; 
            
		public static List<string> ExcludeFiles { get; private set; }
            
		public static string GetPlatformName()
		{
			switch (EditorUserBuildSettings.activeBuildTarget)
			{
			case BuildTarget.Android:
				return "Android";
			case BuildTarget.StandaloneOSX:
				return "OSX";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "Windows";
			case BuildTarget.iOS:
				return "iOS";
			case BuildTarget.WebGL:
				return "WebGL";
			default:
				return Utility.nonsupport;
			}
		}
		/// <summary>
		///     打包输出目录
		/// </summary>
		public static string PlatformBuildPath
		{
			get
			{
				var dir = $"{Utility.buildPath}/{GetPlatformName()}";
				if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

				return dir;
			}
		}
		
		public static bool IsExcluded(string path)
		{
			return ExcludeFiles.Exists(path.EndsWith) || path.EndsWith(".cs") || path.EndsWith(".dll");
		}
		
		public static IEnumerable<string> GetDependencies(string path)
		{
			var set = new HashSet<string>(AssetDatabase.GetDependencies(path, true));
			set.Remove(path);
			set.RemoveWhere(IsExcluded);
			return set.ToArray();
		}
	}
}
