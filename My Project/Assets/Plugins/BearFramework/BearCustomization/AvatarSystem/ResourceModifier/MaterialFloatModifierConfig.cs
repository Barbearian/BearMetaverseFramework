using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [CreateAssetMenu(menuName = "SMRModifierConfig/MaterialFloatModifierConfig")]
    public class MaterialFloatModifierConfig : ModifierConfig
    {
        public int materialIndex;
        public string FloatProperty;
        public float value;
        public override ResourceModifySignal GetSignal()
        {
            return new MaterialFloatModifierSignal() {
                //Address of target change
                ResourceKey = ResourceKey,
                SmrIndex = SmrIndex,
                SubModificationKey = SubModificationKey,
                matIndex = materialIndex,

                //detail on change
                propertyName = FloatProperty,
                value = value

            };
        }

        //private void OnEnable()
        //{
        //    if (SubModificationKey == null || SubModificationKey.Length == 0) {
        //        SubModificationKey = "float";
        //    }
        //}
    }
}