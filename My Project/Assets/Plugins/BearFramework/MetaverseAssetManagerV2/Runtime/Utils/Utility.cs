using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
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
	}
}