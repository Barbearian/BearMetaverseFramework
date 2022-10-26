using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bear
{
    public class Neuron : INeuron
    {

        public List<ISignalReceiver> kids = new List<ISignalReceiver>();
        public void DeLink(ISignalReceiver receiver)
        {
            kids.Remove(receiver);
        }

        private bool isActive = true;
        public bool IsActive { set => isActive = value; get => isActive; }
        private bool IsInhibited;

        public void Link(ISignalReceiver receiver)
        {
            if (!kids.Contains(receiver))
                kids.Add(receiver);
        }

        public void ReceiveSignal(ISignal signal)
        {
            
            
            if (IsInhibited) return;

            if (signal is LinkSignal lsignal)
            {
                Link(lsignal.receiver);
            }
            else
            {

                Debug.Log("The size of receivers are " + kids.Count);
                bool hasNull = false;
                // Debug.Log($"#OutputSignalManager# I have received a {signal.GetType()} signal");
                if (kids != null)
                {
                    for (int i = 0; i < kids.Count; i++)
                    {
                        ISignalReceiver receiver = kids[i];

                        if (receiver == null || (!receiver.IsActive))
                        {
                            Debug.Log($"{i}'th element is null");
                            kids[i] = null;
                            hasNull = true;

                        }
                        else
                        {

                            receiver.ReceiveSignal(signal);
                        }

                    }
                }

                if (hasNull)
                {
                    kids = kids.Where(c => c != null).ToList();
                }
            }
        }


        public void ReceiveCommand(NeuronCommand command)
        {
            switch (command) {
                case NeuronCommand.Inhibit:
                    IsInhibited = true;
                    break;

                case NeuronCommand.Awake:
                    IsInhibited = false;
                    break;
            }
        }

       
    }

    public class LinkSignal : ISignal {
        public ISignalReceiver receiver;
        public LinkSignal(ISignalReceiver receiver) {
            this.receiver = receiver;
        }
    }
}