using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Bear.Asset
{
    public class ResourceManagerService : IResourceManager
    {
        public Dictionary<string, IResource> allResource = new Dictionary<string, IResource>();
        public void Init(IBearFramework framewrok)
        {
           
        }

        public void Load<T>(string key,System.Action<T> DOnComplete) {
            if (allResource.TryGetValue(key, out var value))
            {
                value.LoadResource<T>(DOnComplete);
            }
            else {
                var resource = new Resource(key);
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
    }

    public class Resource: IResource
    {
        public string ResourceName;
        object resource;
        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle handle;

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
    }

    public class AssetReferenceResource : IResource
    {
        AssetReference _reference;
        public AssetReferenceResource(AssetReference reference) {
            _reference = reference;
        }
        public void LoadResource<T>(Action<T> DOnComplete)
        {
            if (_reference!=null) {
                _reference.LoadAssetAsync<T>().Completed += (handler) =>
                {
                    DOnComplete?.Invoke(handler.Result);
                     
                };
            }
        }

        public void UnloadResource()
        {
            if (_reference!=null) {
                _reference.ReleaseAsset();
            }
        }
    }
}