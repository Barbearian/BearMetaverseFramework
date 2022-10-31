using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear{
	using UnityEngine.Networking;
	using System.IO;
	using System;

	
	public class InitializeVersions : Operation
	{
		private readonly List<BuildVersion> _loading = new List<BuildVersion>();
		private readonly List<UnityWebRequestAsyncOperation> _operations = new List<UnityWebRequestAsyncOperation>();

		private BuildVersions _local;
		private string _savePath;

		private Step _step;

		private enum Step
		{
			LoadingLocalVersions,
			LoadingServerVersions,
			UpdateLoading
		}
		
		public override void OnStart()
		{
			_savePath = loader.GetTemporaryPath(Versions.Filename);
			var copy = DownloadAsync(loader.GetPlayerDataPath(Versions.Filename),_savePath);
			_operations.Add(copy);
			_step = Step.LoadingLocalVersions;

		}
		
		private static UnityWebRequestAsyncOperation DownloadAsync(string url,string savePath){
			if(File.Exists(savePath)) File.Delete(savePath);
			
			var request = UnityWebRequest.Get(url);
			request.downloadHandler = new DownloadHandlerFile(savePath);
			return request.SendWebRequest();
		}
		
		private void UpdateLoading(){
			while(_loading.Count >0){
				var version = _loading[0];
				loader.LoadVersion(version);
				_loading.RemoveAt(0);
				
				if(UpdatableSystem.Busy()) return;
			}
			
			Finish();
		}
		
		private void Clear(){
			foreach (var item in _operations) item.webRequest.Dispose();
			
			_operations.Clear();
		}
		
		private bool Progressing(){
			foreach (var item in _operations)
			{
				if(!item.isDone)
					return true;
			}
			
			return false;
		}
		
		private void LoadServerVersions(){
			var versions = new Dictionary<string,BuildVersion>();
			var server = BuildVersions.Load(loader.GetDownloadDataPath(Versions.Filename));
			
			if(_local == null || server.timestamp > _local.timestamp){
				foreach (var item in server.data)
				{
					if(!loader.Exist(item)) continue;
				
				
					if(versions.ContainsKey(item.name)){
						Debug.LogWarningFormat("version exist:{0}", item.name);
						continue;
					}
					
					versions[item.name] = item;
					_loading.Add(item);

				}
			}
			
			if(_local != null){
				foreach (var item in _local.data)
				{
					if(versions.ContainsKey(item.name)) continue;
					
					versions[item.name] = item;
					_loading.Add(item);
				}
			
				loader.ReloadPlayerVersions(_local);
			}
			
			if(_loading.Count == 0){
				Finish("Cannot find Build Version File");
				return;
			}
			
			_step = Step.UpdateLoading;
		}
		
		private void LoadLocalVersions(){
			_local = BuildVersions.Load(_savePath);
			foreach (var item in _local.data)
			{

				if(!loader.Exist(item)){
					var url = loader.GetPlayerDataPath(item.file);
					var savepath = loader.GetDownloadDataPath(item.file);	
					_operations.Add(DownloadAsync(url,savepath));
				}
			}
			
			_step = Step.LoadingServerVersions;
		}
		
		public override void Update()
		{
			if (status != OperationStatus.Processing) return;
			
			if (Progressing()) return;

			Clear();
			
			switch(_step){
				case Step.LoadingLocalVersions:
					LoadLocalVersions();
					break;
				
				case Step.LoadingServerVersions:
					LoadServerVersions();
					break;
				
				case Step.UpdateLoading:
					UpdateLoading();
					break;
					
				default:
					throw new ArgumentOutOfRangeException();
			}

		}
	}
}