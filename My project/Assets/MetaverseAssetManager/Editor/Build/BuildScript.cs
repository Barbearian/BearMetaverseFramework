
namespace Bear.editor{
	using System;
	using UnityEngine;
	public class BuildScripts : MonoBehaviour
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
	}
}