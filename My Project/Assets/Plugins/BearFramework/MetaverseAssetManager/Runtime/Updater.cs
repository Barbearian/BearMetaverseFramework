using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class Updater : MonoBehaviour
    {
        public List<IUpdatable> updates = new List<IUpdatable>();
        public void Update()
        {
            UpdatableSystem._realtimeSinceUpdateStartup = Time.realtimeSinceStartup;
            foreach (var item in updates)
            {
                item.Update();
            }
        }

        public void Clear() { 
            
        }
    }
}