using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine;
public class CreateAssetBundles
{
	
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/AssetBundles";
		if(!Directory.Exists(assetBundleDirectory))
		{
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
			BuildAssetBundleOptions.None, 
			BuildTarget.StandaloneWindows);
	}
	
	[MenuItem("Assets/Debug AssetBundles")]
	static void PrintAllAssetBundlesName()
	{
		AssetDatabase.RemoveUnusedAssetBundleNames();
		
		StringBuilder sb = new StringBuilder();
		foreach (var item in AssetDatabase.GetAllAssetBundleNames())
		{
			sb.Append(item+"\n");
		}
		Debug.Log(sb);
	}
}