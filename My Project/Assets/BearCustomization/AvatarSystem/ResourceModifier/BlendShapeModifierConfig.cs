using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [CreateAssetMenu(menuName = "SMRModifierConfig/BlendShapeModifierConfig")]
    public class BlendShapeModifierConfig : ModifierConfig
    {
        public BlendShapeTuple[] weightData;
        
        public override ResourceModifySignal GetSignal()
        {
            return new BlendShapeModifierSignal() {

                //Address of target change
                ResourceKey = ResourceKey,
                SmrIndex = SmrIndex,
                SubModificationKey = SubModificationKey,

                //detail on change
                weightData = weightData

            };    
        }

        //private void OnEnable()
        //{
        //    if (SubModificationKey == null || SubModificationKey.Length == 0) {
        //        SubModificationKey = "blendshape";
        //    }
            
        //}
    }




}