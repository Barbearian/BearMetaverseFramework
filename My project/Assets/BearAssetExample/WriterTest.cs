using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear{
	using System.IO;
	using System.Text;
	public class WriterTest : MonoBehaviour
	{
		public byte[] arra;
		public string input;
		public string output;
		public int Limit = 1;
		[ContextMenu("TestDownloadLength")]
		public void TestDownloadLength(){
			using(var writer = File.OpenWrite(output)){

				writer.Seek(writer.Length,SeekOrigin.Current);
				var endB = writer.Position;

				writer.Seek(-1,SeekOrigin.End);
				var endA = writer.Position;
				

				
				Debug.Log(endA+" "+endB);
			}
		}
	}
}