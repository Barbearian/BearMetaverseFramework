using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public abstract class ModifierConfig:ScriptableObject
    {
        public string ResourceKey;
        public int SmrIndex;
        public string SubModificationKey;

        private void OnEnable()
        {
            if (SubModificationKey == null || SubModificationKey.Length == 0) {
                SubModificationKey = this.GetType().ToString();
            }
        }

        public abstract ResourceModifySignal GetSignal();
    }

    


}