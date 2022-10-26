using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public enum BodyPart
    {
        Hand_R,
        Hand_L,
        Root
    }

    public interface IBodyAnchor:ISignalReceiver
    {
        string Key { get; set; }
    }

    public class GenerateAtBodyPartSignal : ISignal {
        public string code;
        public GameObject obj;
    }
}