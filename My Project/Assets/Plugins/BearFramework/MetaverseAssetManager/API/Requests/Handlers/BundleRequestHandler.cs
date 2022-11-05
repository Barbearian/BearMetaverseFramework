using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public interface IBundleRequestHandler
	{
		void OnStart();
		void Update();
		void Dispose();
		void WaitForCompletion();
	}
	
	public class LocalBundleRequestHandler : IBundleRequestHandler
	{
		public BundleRequest request { get; set; }
		public string path;

		public void OnStart()
		{
			request = request;
			request.LoadAssetBundle(path);
		}

		public void Update()
		{
		}

		public void Dispose()
		{
		}

		public void WaitForCompletion()
		{
		}
	}
}