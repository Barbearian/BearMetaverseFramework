using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class BodyAnchorManager : MonoBehaviour,ISignalReceiver
    {
        public Dictionary<string, IBodyAnchor> bodyParts = new Dictionary<string, IBodyAnchor>();
        bool _inited;

        bool _isActive;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is GenerateAtBodyPartSignal gsignal) {
                if(TryGetBodyPart(gsignal.code,out var banchor)){
                    banchor.ReceiveSignal(gsignal);
                }
            }
        }

        public bool TryGetBodyPart(BodyPart part, out IBodyAnchor banchor) {
            var key = part.ToString();
            return TryGetBodyPart(key,out banchor);
        }

        public bool TryGetBodyPart(string key, out IBodyAnchor banchor)
        {
            if (bodyParts.TryGetValue(key, out banchor))
            {
                return banchor != null;
            }
            else
            {
                Initialize();
                if (bodyParts.TryGetValue(key, out banchor))
                {
                    return banchor != null;
                }
                else
                {
                    bodyParts[key] = null;
                    return false;
                }
            }
        }

        void Initialize()
        {
            if (!_inited) {
                _inited = true;
                foreach (var item in gameObject.GetComponentsInChildren<IBodyAnchor>(true))
                {
                    bodyParts[item.Key] = item;
                }
            }
        }
    }
}