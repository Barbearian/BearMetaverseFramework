using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class DownloadUpdater : IUpdatable
    {
        
        public void Clear()
        {
            DownloadSystem.ClearAllDownloads();
        }


        public void Update()
        {
            DownloadSystem.UpdateAll();
        }
    }
}