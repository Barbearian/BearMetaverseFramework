using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [System.Serializable]
    public class BlendShapeModifierSignal : ResourceModifySignal
    {
        public BlendShapeTuple[] weightData;
        public override bool Modify(SkinnedMeshRenderer smr)
        {
            foreach (var item in weightData)
            {
                smr.SetBlendShapeWeight(item.index, item.weight);
            }
          
            return true;
        }
    }

    [System.Serializable]
    public class BlendShapeTuple
    {
        public int index;
        public float weight;
    }

    [System.Serializable]
    public class StrBlendShapeModifierSignal : ResourceModifySignal
    {
        public StrBlendShapeTuple[] weightData;
        public override bool Modify(SkinnedMeshRenderer smr)
        {
            foreach (var item in weightData)
            {
                int index = smr.sharedMesh.GetBlendShapeIndex(item.index);
                smr.SetBlendShapeWeight(index, item.weight);
            }

            return true;
        }
    }

    [System.Serializable]
    public class StrBlendShapeTuple
    {
        public string index;
        public float weight;
    }
    [System.Serializable]
    public abstract class MaterialModifierSignal : ResourceModifySignal {
        public int matIndex;

        public override bool Modify(SkinnedMeshRenderer gameObject)
        {
            if (matIndex>=0&& matIndex< gameObject.materials.Length) {
                var mat = gameObject.materials[matIndex];
                return Modify(mat);
            }
            else {
                return false;
            }
            
            
        }

        public abstract bool Modify(Material material);

    }
    [System.Serializable]
    public class MaterialColorModifierSignal : MaterialModifierSignal
    {
        public string propertyName;
        public Color color;
        public override bool Modify(Material material)
        {
            if (material.HasColor(propertyName)) {
                material.SetColor(propertyName,color);
                return true;
            }
            return false;
        }
    }
    [System.Serializable]
    public class MaterialTexModifierSignal : MaterialModifierSignal,IServiceFetcher
    {
        public string propertyName;
        public string tex;
        public override bool Modify(Material material)
        {
            if (material.HasTexture(propertyName))
            {
                if (this.TryGetService<IResourceManager>(out var service)) {
                    service.Load<Texture>(tex, (texture) =>{
                        material.SetTexture(propertyName, texture);
                    });
                    
                }
                return true;
            }
            return false;
        }
    }
    [System.Serializable]
    public class MaterialFloatModifierSignal : MaterialModifierSignal {
        public string propertyName;
        public float value;
        public override bool Modify(Material material)
        {
            if (material.HasFloat(propertyName)) {
                material.SetFloat(propertyName,value);
                return true;
            }
            return false;
        }
    }

    public class CombinedModifierSignal : ResourceModifySignal,IServiceFetcher
    {
        public string[] Signals;
        public override bool Modify(SkinnedMeshRenderer gameObject)
        {
            return false;
        }

        public void ApplySignal(System.Action<ISignal> OnReceiveSignal) {

            if (this.TryGetService<AvatarCustomizationService>(out var service)) {
                foreach (var item in Signals)
                {
                    if (service.TryGetSignal(item,out var signal))
                    {
                        OnReceiveSignal.Invoke(signal);
                    }
                }
                
            }
            
        }

        public void ApplySignalWithKey(System.Func<ResourceModifySignal, bool> OnReceiveSignal,string ResourceKey)
        {
            if (this.TryGetService<AvatarCustomizationService>(out var service)) {
                foreach (var item in Signals)
                {
                    if (
                        service.TryGetSignal(item, out ISignal signal)
                        &&
                        signal is ResourceModifySignal rmsignal
                        &&
                        rmsignal.ResourceKey.Equals(ResourceKey)
                        ) {
                        OnReceiveSignal.Invoke(rmsignal);
                    }
                }
            
            }

        }

        
    }


}