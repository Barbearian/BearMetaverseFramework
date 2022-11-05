using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.Networking;
	using System;
	public interface IInitializeRequestHandler{
		void OnStart();
		void OnUpdated();
	}
	public class InitializeRequestHandlerRuntime : IInitializeRequestHandler
	{
		private readonly Queue<Version> _queue = new Queue<Version>();
		private readonly List<UnityWebRequest> _requests = new List<UnityWebRequest>();
		private Versions _downloadVersions;

		private Step _step = Step.LoadPlayerVersions;
		private UnityWebRequest _unityWebRequest;

		private InitializeRequest request { get; set; }

		public void OnStart(){
			_unityWebRequest = UnityWebRequest.Get(request.loader.GetPlayerDataPath(PlayerAssets.Filename));
			_unityWebRequest.SendWebRequest();
			_step = Step.LoadPlayerAssets;
		}
		
		public void OnUpdated(){
			switch (_step)
			{
			case Step.LoadPlayerAssets:
				UpdateLoadingPlayerAssets();
				break;
			case Step.LoadVersionsHeader:
				UpdateLoadVersionsHeader();
				break;
			case Step.LoadPlayerVersions:
				UpdateLoadPlayerVersions();
				break;
			case Step.LoadVersionsContent:
				UpdateLoadVersions();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		
		//download player data
		private void UpdateLoadingPlayerAssets(){
			if(!_unityWebRequest.isDone) return;
			if(!string.IsNullOrEmpty(_unityWebRequest.error)){
				request.SetResult(Request.Result.Failed,_unityWebRequest.error);
				return;
			}
			
			request.loader.PlayerAssets =  Utility.LoadFromJson<PlayerAssets>(_unityWebRequest.downloadHandler.text);
			_unityWebRequest.Dispose();
			
			_unityWebRequest = UnityWebRequest.Get(request.loader.GetPlayerDataURl(Versions.Filename));
			_unityWebRequest.SendWebRequest();
			_step = Step.LoadVersionsHeader;

		}
		private void UpdateLoadVersionsHeader(){
			var Loader = request.loader;
			//if download faill then finish with error
			if(!_unityWebRequest.isDone) return;
			if(!string.IsNullOrEmpty(_unityWebRequest.error)){
				request.SetResult(Request.Result.Failed,_unityWebRequest.error);
				return;
			}
			
			//get versions from download
			var json = _unityWebRequest.downloadHandler.text;
			Logger.D($"LoadingVersionHeader {json}");
			request.loader.Versions = Utility.LoadFromJson<Versions>(json);
			_unityWebRequest.Dispose();
			
			foreach (var version in Loader.Versions.data)
			{
				if (Loader.IsDownloaded(version)) continue;
				var url = Loader.GetPlayerDataURl(version.file);
				var savePath = Loader.GetDownloadDataPath(version.file);
				var unityWebRequest = UnityWebRequest.Get(url);
				unityWebRequest.downloadHandler = new DownloadHandlerFile(savePath);
				unityWebRequest.SendWebRequest();
				_requests.Add(unityWebRequest);
			}
			
			_step = Step.LoadPlayerVersions;
		}
		private void UpdateLoadPlayerVersions(){
			//Downloading
			for (int i = 0; i < _requests.Count; i++) {
				var unityWebRequest = _requests[i];
				if (!unityWebRequest.isDone) return;
				_requests.RemoveAt(i);
				i--;
				if (!string.IsNullOrEmpty(unityWebRequest.error))
					request.SetResult(Request.Result.Failed, unityWebRequest.error);
				unityWebRequest.Dispose();
			}
			
			//set up versions
			var path = request.loader.GetDownloadDataPath(Versions.Filename);
			_downloadVersions  = Utility.LoadFromJson<Versions>(path);
			if(_downloadVersions != null && _downloadVersions.timestamp > request.loader.Versions.timestamp)
				request.loader.Versions = _downloadVersions;
					
			foreach (var version in request.loader.Versions.data)
				_queue.Enqueue(version);
			_step = Step.LoadVersionsContent;
			
		}
		private void UpdateLoadVersions(){
			var Loader =request.loader;
			while(_queue.Count > 0){
				var version = _queue.Dequeue();
				var path = Loader.GetDownloadDataPath(version.file);
				var manifest = Utility.LoadFromJson<Manifest>(path);
				
				manifest.build = version.build;
				manifest.name = version.file;
				version.manifest = manifest;
				
				if(Scheduler.Busy) return;
			}
		}
		internal static IInitializeRequestHandler CreateInstance(InitializeRequest initializeRequest)
		{
			return new InitializeRequestHandlerRuntime {request = initializeRequest};
		}
		
		private enum Step
		{
			LoadPlayerVersions,
			LoadPlayerAssets,
			LoadVersionsHeader,
			LoadVersionsContent
		}
	}
}