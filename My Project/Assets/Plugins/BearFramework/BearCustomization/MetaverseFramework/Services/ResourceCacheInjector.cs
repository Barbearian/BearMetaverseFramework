using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.Events;
	public class ResourceCacheInjector : MonoBehaviour
	{
		public List<GameObject> avatars;
		public UnityEvent DOnLoaded;
		public void Awake(){
			ResourceManagerService.CreateResource = Make;
			foreach (var avatar in avatars)
			{
				Debug.Log(avatar.name);
				ResourceCache.Set(avatar.name,avatar);
			}
			
			DOnLoaded.Invoke();
		}
		
		public IResource Make(){
			return new CacheResource();
		}
	}
}