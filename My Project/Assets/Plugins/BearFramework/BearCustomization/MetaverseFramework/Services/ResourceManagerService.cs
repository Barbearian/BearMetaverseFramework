using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Bear
{
    public class ResourceManagerService : IResourceManager
	{
		public static Func<IResource> CreateResource {get;set;} = Resource.Make;
        public Dictionary<string, IResource> allResource = new Dictionary<string, IResource>();
        public void Init(IBearFramework framewrok)
        {
	        allResource = new Dictionary<string, IResource>();
        }
        
	

		public void Load<T>(string key,System.Action<T> DOnComplete) {
			
            if (allResource.TryGetValue(key, out var value))
            {
	            //  Debug.Log(key+" Exists");
	            //   value.SetPath(key);
                value.LoadResource<T>(DOnComplete);
            }
            else {
	            var resource = CreateResource();
	            string mykey = key;
	            resource.SetPath(mykey);
	            // Debug.Log(key+" is loaded ");
                allResource[key] = resource;
                resource.LoadResource<T>(DOnComplete);
            }
        }

        

        public void UnloadResource(string key) {
            if (allResource.TryGetValue(key, out var value))
            {
                value.UnloadResource();
            }
        }


    }

    public interface IResource {
        void LoadResource<T>(System.Action<T> DOnComplete);
	    void UnloadResource();
	    void SetPath(string path);
    }

    public class Resource: IResource
	{
		public static IResource Make(){
			return new Resource();
		}
        public string ResourceName;
        object resource;
        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle handle;

	    public Resource() {
	    }

        public Resource(string rname) {
            ResourceName = rname.Replace("\r", "");
        }
        private UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<T> LoadResource<T>() {
           // Debug.Log(ResourceName);
            var rs = Addressables.LoadAssetAsync<T>(ResourceName);
            rs.Completed += (rs) => {
                resource = rs.Result;
                handle = rs;
            };
            return rs;
        }

        public void LoadResource<T>(System.Action<T> DOnComplete) {
            if (resource != null && resource is T rs)
            {
                DOnComplete?.Invoke(rs);
            }
            else {
                LoadResource<T>().Completed += (rs) => {
                    DOnComplete?.Invoke(rs.Result);
                };
            }

        }

        public void UnloadResource() {
            resource = null;
            Addressables.Release(handle);
        }
        
	    public void SetPath(string path){
	    	ResourceName = path;
	    }
    }

   
}