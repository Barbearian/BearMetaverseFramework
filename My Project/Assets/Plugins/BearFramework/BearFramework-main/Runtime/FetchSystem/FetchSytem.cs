using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public interface IFetcher<T> {
        public T GetValue();
    }

    public class InstanceFetcher : IFetcher<GameObject>
    {
        public GameObject Prefab;
        bool fetched = false;
        private GameObject Instance;
        public InstanceFetcher(GameObject prefab)
        {
            this.Prefab = prefab;
        }

        public GameObject GetValue()
        {
            if (!fetched)
            {
                Instance = Object.Instantiate(Prefab);
                fetched = true;
            }

            return Instance;
        }
    }
}