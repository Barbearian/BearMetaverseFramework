using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class BodyAnchor : MonoBehaviour, IBodyAnchor
    {
        public BodyPart myPart;
        GameObject generated;

        public string Key { get => myPart.ToString(); set{ } }

        public bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = false; }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is GenerateAtBodyPartSignal gSignal) {
                if (generated != null) {
                    Destroy(generated);
                }
                generated = Instantiate(gSignal.obj,transform);
            }
        }
    }

   
}