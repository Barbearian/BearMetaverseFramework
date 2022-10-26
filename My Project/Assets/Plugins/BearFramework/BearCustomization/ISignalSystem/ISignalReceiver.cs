using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public interface ISignalReceiver
    {
        public void ReceiveSignal(ISignal signal);
        public bool IsActive { get; set; }
    }



    public interface INeuron :ISignalReceiver{
        public void Link(ISignalReceiver receiver);
        public void ReceiveCommand(NeuronCommand command);
        //public void DeLink(ISignalReceiver receiver);
    }

    public enum NeuronCommand { 
        Inhibit,
        Awake,
        Die,
    }

    public class ActionSignalReceiver : ISignalReceiver
    {
        System.Action<ISignal> action;
        public bool IsActive { get => true; set { } }

        public ActionSignalReceiver(System.Action<ISignal> action) {
            this.action = action;
        }
        public void ReceiveSignal(ISignal signal)
        {
            action?.Invoke(signal);
        }
    }


}