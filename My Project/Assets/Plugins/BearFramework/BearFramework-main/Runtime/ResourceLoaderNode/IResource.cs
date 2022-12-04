using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using Cysharp.Threading.Tasks;
	
	public interface IBearResource
	{
		void LoadResource<T>(System.Action<T> DOnComplete);

		UniTask<T> LoadResourceAsync<T>(string address);

		void UnloadResource();
	}
}