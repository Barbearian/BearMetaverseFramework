using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public interface IResourceManager: IBearService
    {
        void Load<T>(string key, System.Action<T> DOnComplete);

        void UnloadResource(string key);
    }
}
