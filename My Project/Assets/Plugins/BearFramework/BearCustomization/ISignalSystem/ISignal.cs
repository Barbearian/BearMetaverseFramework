using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public interface ISignal
    {
        
    }

    public class StringSignal : ISignal {
        
        public string signal = "";
    }
}