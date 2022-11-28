using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear{
	using System.Text.RegularExpressions;
	public static class ResourceCache
	{
		public static Dictionary<string,GameObject> Cache = new Dictionary<string,GameObject>();
		public static GameObject Get(string key){
			//	Debug.Log("Get "+key);
			var path = CacheUtil.GetPath(key);
			if(path.Length>0){
				var rootKey = path[0];
				if(Cache.TryGetValue(rootKey,out var val)){
					return val.GetChildren(path,true);
				}
			}

			return default;
		} 
		
		public static void Set(string key,GameObject obj){
			
			Cache[key] = obj;
		} 
		
		public static void Remove(string key){
			Cache.Remove(key);
		} 
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Init(){
			Cache = new Dictionary<string, GameObject>();
		}
	}
	
	public class CacheResource:IResource{
		private string key; 
		private GameObject asset;
		public void SetPath(string key){
			//Debug.Log("Set path to "+key +"_______________________________");
			this.key = key;
		}
		
		
		public void LoadResource<T>(System.Action<T> DOnComplete) {
			
			//Debug.Log("My key is "+key);
			var rs = ResourceCache.Get(key);
			if(rs is T value){
				asset = rs;
				DOnComplete.Invoke(value);
			}
			

		}
		
		public void UnloadResource() {
			ResourceCache.Cache.Remove(key);
		}
		
	}
	
	public static class CacheUtil{
		public static string pattern = @"\[(.*)\]";
		public static string[] GetPath(string key){
			List<string> arr = new List<string>();
			Regex reg =new Regex(pattern);
			var match = reg.Match(key);
			if(match.Success){
				var group1 = match.Groups[0].ToString();
				var group2 = match.Groups[1].ToString();
				arr.Add(key.Replace(group1,""));
				var splited = group2.Split("/",System.StringSplitOptions.RemoveEmptyEntries);
				arr.AddRange(splited);
			}else{
				arr.Add(key);
			}
			
			return arr.ToArray();
		}
		
		public static GameObject GetChildren(this GameObject obj,IEnumerable<string> childrenName,bool ignoreFirst = false){
			GameObject curr = obj;

			foreach (var item in childrenName)
			{
				if(ignoreFirst){
					ignoreFirst = false;
					continue;
				}
				
				if(curr == null){
					return null;
				}else{
					curr = curr.transform.Find(item).gameObject;
				}
			}
			
			return curr;
		}
	}
}