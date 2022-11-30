using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ResourceModifierManager : MonoBehaviour, ILocalFrameworkFetcher, ISignalReceiver
    {
        public Dictionary<(string, string), ResourceModifySignal> dic = new Dictionary<(string, string), ResourceModifySignal>();
        
        IFrameworkFetcher _fetcher;

        bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public IFrameworkFetcher GetFetcher()
        {
            if (_fetcher == null)
            {
                _fetcher = new FrameworkFetcher(gameObject);
            }
            return _fetcher;
        }

        private void Awake()
        {
            this.RegisterMailBox(AvatarMakingKeywords.ResourceModifier,this);
        }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is ResourceModifySignal rmsignal)
            {
               
            }
            
        }

        
        
    }

    public class ModifierDictionary { 
        
    }

    
}