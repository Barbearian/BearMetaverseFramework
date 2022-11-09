using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System.Diagnostics;
	using UnityEngine.SceneManagement;
	public class Record
	{
		public string name;
		public float elapsed;
		public int frames;
		public ulong size;
		public string loadScene;
		public string unloadScene;
		public int loads;
		public int unloads;
		public int refCount;
		public List<string> loadScenes = new List<string>();
		public List<string> unloadScenes = new List<string>();
		public List<string> children = new List<string>();
		private int _startFrame;
		private float _startTime;

		public void BeginSample()
		{
			_startTime = Time.realtimeSinceStartup;
			_startFrame = Time.frameCount;
		}

		public void EndSample()
		{
			elapsed = (Time.realtimeSinceStartup - _startTime) * 1000;
			frames = Time.frameCount - _startFrame + 1;
		}
	}
	
	public class Recorder
	{
		public static readonly List<Record> Loads = new List<Record>();
		public static readonly List<Record> Unloads = new List<Record>();
		public static readonly List<Record> Current = new List<Record>();
		private static readonly Dictionary<string, Record> _records = new Dictionary<string, Record>();
		private static readonly Dictionary<int, Record> _frameWithRecords = new Dictionary<int, Record>();
		
		public static Record Get(string name)
		{
			return _records.TryGetValue(name, out var value) ? value : null;
		}
		
		[Conditional("DEBUG")]
		public static void BeginSample(string name, LoadRequest request){
			//var sceneName = SceneRequest.main?.path;
			var sceneName = SceneManager.GetActiveScene().name;
			if(!_records.TryGetValue(name,out var value)){
				value = new Record();
				switch(request){
				
				}
			}
		}
		
		[Conditional("DEBUG")]
		public static void EndSample(string name)
		{
			if (!_records.TryGetValue(name, out var value)) return;
			value.EndSample();
		}
		
		[Conditional("DEBUG")]
		public static void Unload(string name)
		{
			//if (!_records.TryGetValue(name, out var value)) return;
			//var sceneName = SceneRequest.main?.path;
			//if (string.IsNullOrEmpty(sceneName)) sceneName = SceneManager.GetActiveScene().path;
			//value.unloads++;
			//value.unloadScenes.Add(sceneName);
			//value.refCount--;
			//if (value.refCount != 0) return;
			//value.unloadScene = sceneName;
			//Current.Remove(value);
			//Unloads.Add(value);
		}
	}
}