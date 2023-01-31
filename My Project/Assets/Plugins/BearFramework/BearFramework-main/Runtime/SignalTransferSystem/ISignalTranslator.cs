using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public interface ISignalTranslator
    {
        public bool TryTranslate(INodeSignal signal, out INodeSignal rs);
    }

    public class TypeSignalTranslator :ISignalTranslator
    { 
        public Dictionary<string, INodeSignal> signalMap = new Dictionary<string, INodeSignal>();



        public bool TryTranslate(INodeSignal signal, out INodeSignal CanTranslate)
        {
            var key = signal.GetType().ToString();

            return signalMap.TryGetValue(key,out CanTranslate);
        }
    }

    public class KeySignalTranslator : ISignalTranslator
    {

        public Dictionary<string, INodeSignal> signalMap = new Dictionary<string, INodeSignal>();

        public bool TryTranslate(INodeSignal signal, out INodeSignal rs)
        {
            if (signal is StrKeySignal sks)
            {
                var key = sks.key;
                return signalMap.TryGetValue(key,out rs);
            }

            rs= null;
            return false; 
        }

        public class StrKeySignal : INodeSignal {
            public string key;
        }
    }


}