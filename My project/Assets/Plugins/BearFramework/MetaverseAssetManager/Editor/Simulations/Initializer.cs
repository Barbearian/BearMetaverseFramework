using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bear.editor{
	public class Initializer : MonoBehaviour
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RuntimeInitializeOnLoad()
		{
			Debug.Log("Hello");
		}
		
		[InitializeOnLoadMethod]
		private static void InitializeOnLoad()
		{
			Settings.GetDefaultSettings().Initialize();
		}
	}
}