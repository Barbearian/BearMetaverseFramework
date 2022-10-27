using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class AvatarMakingKeywords {
        public const string ResourceMaker = "ResourceMaker";
        public const string ResourceModifier = "ResourceModifier";
        public const string OnResourceMade = "OnResourceMade";
    }

    public class BodyPartKeywords {
        public const string Body = "Body";
        public const string Outfit = "Outfit";
    }

   
	[System.Serializable]
    public class ResourceMakeSignal : ISignal
    {
        public string id;
        public string typeName;
        public string[] resourceName;
    }

    public class OnResourceMadeSignal : ISignal
    {
        public string typeName;
        public GameObject resource;
    }

    public abstract class ResourceModifySignal : ISignal
    {
        public string id;
        public string ResourceKey;
        public int SmrIndex = 0;
        public string SubModificationKey;
        public abstract bool Modify(SkinnedMeshRenderer gameObject);

        public override bool Equals(object obj)
        {
            if (obj is ResourceModifySignal rms)
            {
                return (ResourceKey, SmrIndex, SubModificationKey) == (rms.ResourceKey, rms.SmrIndex,rms.SubModificationKey);

            }
            return false;
        }

        public override int GetHashCode()
        {
            return (ResourceKey, SmrIndex,SubModificationKey).GetHashCode();
        }
    }

    public interface IResourceContainer {
        public Dictionary<string, GameObject> Resources { get; }
    }
}