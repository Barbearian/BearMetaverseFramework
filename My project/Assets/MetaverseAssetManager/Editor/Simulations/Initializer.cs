using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bear.editor{
	public class Initializer : MonoBehaviour
	{
		[InitializeOnLoadMethod]
		private static void InitializeOnLoad()
		{
			Settings.GetDefaultSettings().Initialize();
		}
	}
}