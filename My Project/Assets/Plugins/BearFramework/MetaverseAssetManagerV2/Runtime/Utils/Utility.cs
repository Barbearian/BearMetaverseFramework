using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
	using System;
	public static class Utility 
	{
		public static string ToHash(byte[] data)
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
		
		public static string ComputeHash(byte[] bytes){
			var data = MD5.Create().ComputeHash(bytes);
			return ToHash(data);
		}
		
		public static void CreateDirectoryIfNecessary(string path){
			var dir = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(dir) || Directory.Exists(dir)) return;

			Directory.CreateDirectory(dir);
		} 
		
		public static T LoadFromFile<T>(string filename) where T: ScriptableObject
		{
			if(!File.Exists(filename)) return ScriptableObject.CreateInstance<T>();
			
			var json = File.ReadAllText(filename);
			var asset = ScriptableObject.CreateInstance<T>();
			try
			{
				JsonUtility.FromJsonOverwrite(json, asset);
			}
			catch (Exception e)
			{
				Logger.E(e.Message);
				File.Delete(filename);
			}
				
			return asset;

		}
	}
}