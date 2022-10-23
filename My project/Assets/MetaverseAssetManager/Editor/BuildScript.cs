using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.editor{
	using System;
	using System.IO;
	public class BuildScript
	{
		public static Action<BuildTask> postprocessBuildBundles { get; set; }
		public static Action<BuildTask> preprocessBuildBundles { get; set; }

		public static void BuildBundles(BuildTask task)
		{
			preprocessBuildBundles?.Invoke(task);
			task.Run();
			postprocessBuildBundles?.Invoke(task);
		}
		public static void BuildBundles()
		{
			BuildBundles(new BuildTask());
		}
		
		public static void CopyToStreamingAssets()
		{
			var settings = Settings.GetDefaultSettings();
			var destinationDir = Settings.BuildPlayerDataPath;
			if (Directory.Exists(destinationDir)) Directory.Delete(destinationDir, true);

			Directory.CreateDirectory(destinationDir);
			var versions = BuildVersions.Load(Settings.GetBuildPath(Versions.Filename));
			var bundles = settings.GetBundlesInBuild(versions);
			foreach (var bundle in bundles) Copy(bundle.nameWithAppendHash, destinationDir);

			foreach (var build in versions.data) Copy(build.file, destinationDir);

			versions.streamingAssets = bundles.ConvertAll(o => o.nameWithAppendHash);
			versions.offlineMode = settings.scriptPlayMode != ScriptPlayMode.Increment;
			File.WriteAllText($"{destinationDir}/{Versions.Filename}", JsonUtility.ToJson(versions));
		}
		
		private static void Copy(string filename, string destinationDir)
		{
			var from = Settings.GetBuildPath(filename);
			if (File.Exists(from))
			{
				var dest = $"{destinationDir}/{filename}";
				File.Copy(from, dest, true);
			}
			else
			{
				Debug.LogErrorFormat("File not found: {0}", from);
			}
		}
		
		public static void ClearBuild()
		{
			if (!UnityEditor.EditorUtility.DisplayDialog("提示", "清理构建数据将无法正常增量打包，确认清理？", "确定")) return;

			var buildPath = Settings.PlatformBuildPath;
			Directory.Delete(buildPath, true);
		}
		
		
	}
}