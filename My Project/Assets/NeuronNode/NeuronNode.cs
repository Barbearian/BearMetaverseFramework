
using UnityEngine;

namespace Bear
{
    public struct NeuronNode
    {
        public uint activationValue;
    }

    public struct NeuronNodeGroup { 
        public NeuronNode[] nodes;

        public uint GetActivation(uint key) {
            uint rs = 0;
            return rs;
        }
    }
}