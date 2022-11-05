using System.Collections;
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
	
	public class AssetRequestHandler : IAssetRequestHandler
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
		
		private void LoadAsset(){}
		
		public void Dispose(){
			
		}
		
		public void WaitForCompletion(){}
	}
}