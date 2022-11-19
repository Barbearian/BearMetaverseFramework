using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class SkinnedMeshContainer : MonoBehaviour,ISkinnedMeshContainer
    {
        public SkinnedMeshRenderer[] renderers = new SkinnedMeshRenderer[0];

        public SkinnedMeshRenderer[] GetSkinnedMeshes()
        {
            return renderers;
        }

       


    }

    public interface ISkinnedMeshContainer {
        public SkinnedMeshRenderer[] GetSkinnedMeshes();
    }
}