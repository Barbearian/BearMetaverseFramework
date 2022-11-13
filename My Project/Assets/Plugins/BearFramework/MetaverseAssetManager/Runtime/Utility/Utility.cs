using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Bear
{
	public static class Utility
	{
		public const string buildPath = "Bundles";
		public const string nonsupport = "Nonsupport";
		public static string GetPlatformName()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.WindowsPlayer:
				return "Windows";
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			case RuntimePlatform.WebGLPlayer:
				return "WebGL";
			default:
				return nonsupport;
			}
		}		
		
		public static string ToHash(IEnumerable<byte> data)
		{
			var sb = new StringBuilder();
			foreach (var t in data) sb.Append(t.ToString("x2"));

			return sb.ToString();
		}
		public static string ComputeHash(string filename)
		{
			if (!File.Exists(filename)) return string.Empty;

			using (var stream = File.OpenRead(filename))
			{
				return ToHash(MD5.Create().ComputeHash(stream));
			}
		}
		
		public static void CreateDirectoryIfNecessary(string path)
		{
			var dir = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(dir) || Directory.Exists(dir)) return;

			Directory.CreateDirectory(dir);
		}
		
		public static T LoadScriptableObjectWithJson<T>(string filename) where T:ScriptableObject
		{
			if (!File.Exists(filename))
			{
				Debug.Log($"File {filename} Doesn't Exist, Created a new one");
				return ScriptableObject.CreateInstance<T>();
			}
			
			var json = File.ReadAllText(filename);
			var asset = ScriptableObject.CreateInstance<T>();
			try{
				JsonUtility.FromJsonOverwrite(json, asset);
				Debug.Log("Loaded file\n"+json);
			}catch (Exception e){
				Debug.LogException(e);
				File.Delete(filename);
			}
			
			return asset;

		}
	}

}