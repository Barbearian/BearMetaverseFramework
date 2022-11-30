using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bear
{
    public class NeuroSignalProcessor : MonoBehaviour
    {
        public string code;
        public NeuroSignalProcessorData npd = new NeuroSignalProcessorData();


        private void Awake()
        {
            npd.Link(code);
        }

        private void OnEnable()
        {
            npd.IsActive = true;   
        }

        private void OnDisable()
        {
            npd.IsActive = false;
        }

    }

    [System.Serializable]
    public class NeuroSignalProcessorData : ISignalReceiver, IGlobalMailSender
    {
        public UnityEvent DOnReceivedSignal = new UnityEvent();
        public NeuroSignalProcessorData() {
            
        }

        public void Link(string code) {
            this.SendGlobalMail(code, new LinkSignal(this));
        }

        bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void ReceiveSignal(ISignal signal)
        {
            DOnReceivedSignal?.Invoke();
        }
    }
}
