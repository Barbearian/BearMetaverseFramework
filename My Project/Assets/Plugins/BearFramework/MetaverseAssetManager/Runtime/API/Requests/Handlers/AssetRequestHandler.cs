﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public interface IAssetRequestHandler
	{
		void OnStart();
		void Update();
		void Dispose();
		void WaitForCompletion();
	}
	
	public class AssetRequestHandlerRuntime : IAssetRequestHandler
	{
		private Dependencies _dependencies;
		private AssetRequest _request { get; set; }

		public void OnStart()
		{
			_dependencies = _request.loader.LoadDependenciesAsync(_request.info);
		}
		
		public void Update()
		{
			_dependencies.Update();
			_request.progress = _dependencies.progress;
			if (!_dependencies.isDone) return;
			LoadAsset();
		}
		
		private void LoadAsset(){
			if(!_dependencies.CheckResult(_request))
				return;
				
			var assetBundle = _dependencies.assetBundle;
			//on fail to load bundle
			if (assetBundle == null)
			{
				_request.SetResult(Request.Result.Failed, "assetBundle == null");
				return;
			}
			
			var info = _request.info;
			var type = _request.type;
			var path = info.path;
			if(_request.isAll)
				_request.assets = assetBundle.LoadAssetWithSubAssets(path, type);
			else
				_request.asset = assetBundle.LoadAsset(path, type);
				
			SetResult();


		}	
		
		private void SetResult(){
			if(_request.isAll){
				if(_request.assets == null){
					_request.SetResult(Request.Result.Failed, "assets == null");
					return;	
				}
			}else{
				if (_request.asset == null)
				{
					_request.SetResult(Request.Result.Failed, "asset == null");
					return;
				}
			}
			
			_request.SetResult(Request.Result.Success);

		}
		
		public void Dispose()
		{
			_dependencies.Release();
		}
		
		public void WaitForCompletion(){
			_dependencies.WaitForCompletion();
			if (_request.result == Request.Result.Failed) return;
			LoadAsset();

		}
		
		public static IAssetRequestHandler CreateInstance(AssetRequest assetRequest)
		{
			return new AssetRequestHandlerRuntime {_request = assetRequest};
		}
	}
}