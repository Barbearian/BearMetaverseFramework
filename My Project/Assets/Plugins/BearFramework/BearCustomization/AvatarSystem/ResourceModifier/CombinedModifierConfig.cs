using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [CreateAssetMenu(menuName = "SMRModifierConfig/CombinedModifierConfig")]
    public class CombinedModifierConfig : ModifierConfig
    {
        public ModifierConfig[] configs;

        public override ResourceModifySignal GetSignal()
        {

            return new CombinedModifierSignal()
            {
                //Address of target change
                ResourceKey = ResourceKey,
                SmrIndex = SmrIndex,
                SubModificationKey = SubModificationKey,

                ////detail
                //Signals = GetSignals()
            };
        }

        public ResourceModifySignal[] GetSignals() {
            ResourceModifySignal[] signals = new ResourceModifySignal[configs.Length];
            for (int i = 0; i < configs.Length; i++)
            {
                signals[i] = configs[i].GetSignal();
            }
            return signals;
        }


    }
}