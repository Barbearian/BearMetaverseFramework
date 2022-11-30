using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [CreateAssetMenu(menuName = "SMRModifierConfig/MaterialColorModifierConfig")]
    public class MaterialColorModifierConfig : ModifierConfig
    {
        public int materialIndex;
        public string ColorPropertyName;
        public Color color;
        public override ResourceModifySignal GetSignal()
        {
            return new MaterialColorModifierSignal() {
                //Address of target change
                ResourceKey = ResourceKey,
                SmrIndex = SmrIndex,
                SubModificationKey = SubModificationKey,
                matIndex = materialIndex,

                //detail on change
                propertyName = ColorPropertyName,
                color = color
            };
        }

        //private void OnEnable()
        //{
        //    if ( SubModificationKey == null || SubModificationKey.Length == 0) {
        //        SubModificationKey = "color";
        //    }
        //}
    }
}