using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class DebugService : LocalBearService,ISignalSender,ISignalReceiver
    {

        public string address;
        public string targetAddress;
        public string[] messages;

        bool _isActive;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void Awake()
        {
            this.SendSignal(new RegisterSignal(address,this));
            foreach (var item in messages)
            {
                this.SendSignal(new DeliverSignal(targetAddress, new DebugSignal(item)));
            }
        }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is DebugSignal dsignal) {
                Debug.Log(address +" Received : "+dsignal.data);
            }
        }
    }

    public class DebugSignal :ISignal{
        public string data;
        public DebugSignal(string data) {
            this.data = data;
        }
    }
}