using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public interface ISignalTranslator
    {
        public INodeSignal Translate(INodeSignal signal);
    }

    public class TypeSignalTranslator :ISignalTranslator
    { 
        public Dictionary<string, INodeSignal> signalMap = new Dictionary<string, INodeSignal>();

        public INodeSignal Translate(INodeSignal signal)
        {

            var key = signal.GetType().ToString();
            if (signalMap.TryGetValue(key,out var rs)) {
                return rs;
            }
            return null;
        }
    }
}