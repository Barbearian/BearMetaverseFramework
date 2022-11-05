using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	using Object = UnityEngine.Object;

	public class AssetLoaderRequest : Request
	{
		public AssetLoader loader;
	}
	
	public static class AssetLoaderRequestSystem{
		public static Func<AssetRequest, AssetRequestHandler> CreateHandler { get; set; }

		private static readonly Queue<AssetRequest> Unused = new Queue<AssetRequest>();
		private static readonly Dictionary<string, AssetRequest> Loaded = new Dictionary<string, AssetRequest>();
		
		public static void RemoveAssetRequest(this AssetLoader loader, AssetRequest request)
		{
			Loaded.Remove($"{request.info.path}[{request.type.Name}]");
			Unused.Enqueue(request);
		}
		
		internal static T GetAssets<T>(this AssetLoader loader,string path) where T : Object
		{
			
			if (!loader.Versions.TryGetAsset(path, out var info)) return null;
			if (!Loaded.TryGetValue($"{info.path}[{typeof(T).Name}]", out var request)) return null;
			return request.asset as T;
		}
		
		internal static T[] GetAllAssets<T>(this AssetLoader loader,string path) where T : Object
		{
			if (!loader.Versions.TryGetAsset(path, out var info)) return null;
			if (!Loaded.TryGetValue($"{info.path}[{typeof(T).Name}]", out var request)) return null;
			return request.assets as T[];
		}
		
		internal static AssetRequest LoadAssetRequest(this AssetLoader loader,string path, Type type, bool isAll = false){
			if (!loader.Versions.TryGetAsset(path, out var info))
			{
				Logger.E($"File not found:{path}");
				return null;
			}
			
			var key = $"{info.path}[{type.Name}]";
			if (!Loaded.TryGetValue(key, out var request)){
				request = Unused.Count > 0 ? Unused.Dequeue() : new AssetRequest();
				request.loader = loader;
				request.Reset();
				request.type = type;
				request.info = info;
				request.isAll = isAll;
				request.path = info.path;
				request.handler = CreateHandler(request);
				Loaded[key] = request;
			}
			
			request.LoadAsync();
			return request;
		}
	}
}