using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class InitializeRequest : AssetLoaderRequest
	{
		private readonly IInitializeRequestHandler _handler;
		public InitializeRequest()
		{
			_handler = CreateHandler(this);
		}
		
		public static Func<InitializeRequest, IInitializeRequestHandler> CreateHandler { get; set; } = InitializeRequestHandlerRuntime.CreateInstance;
		
		protected override void OnUpdated()
		{
			_handler.OnUpdated();
		}
		
		protected override void OnStart()
		{
			_handler.OnStart();
		}
	}
}