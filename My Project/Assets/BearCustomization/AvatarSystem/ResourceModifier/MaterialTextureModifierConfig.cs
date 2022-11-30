using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [CreateAssetMenu(menuName = "SMRModifierConfig/MaterialTextureModifierConfig")]
    public class MaterialTextureModifierConfig : ModifierConfig
    {
        public int materialIndex;
        public string TexPropertyName;
        public string tex;
        public override ResourceModifySignal GetSignal()
        {
            return new MaterialTexModifierSignal() {
                //Address of target change
                ResourceKey = ResourceKey,
                SmrIndex = SmrIndex,
                SubModificationKey = SubModificationKey,
                matIndex = materialIndex,

                //Spacific change
                propertyName = TexPropertyName,
                tex = tex,

            };
        }

        //private void OnEnable()
        //{
            
        //}
    }
}