using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class ISignalDistributor : MonoBehaviour,ISignalReceiver
    {
        public Dictionary<string, ISignalReceiver> receivers = new Dictionary<string, ISignalReceiver>();

        public bool IsActive { get =>gameObject != null; set { } }

        private void Awake()
        {
            Register<RegisterSignalReceiverSignal>(new ActionSignalReceiver((signal)=> {
                if (signal is RegisterSignalReceiverSignal rsignal) {
                    receivers[rsignal.type.ToString()] = rsignal.receiver;
                }
            }));

            Register<DeregisterSignalReceiverSignal>(new ActionSignalReceiver((signal) => {
                if (signal is DeregisterSignalReceiverSignal rsignal)
                {
                    receivers.Remove(rsignal.type.ToString());
                }
            }));

            Register<InvokeReceiverSignal>(new ActionSignalReceiver((signal) => {
                if (signal is InvokeReceiverSignal rsignal)
                {
                    if (receivers.TryGetValue(rsignal.invokation,out var receiver)) {
                        receiver.ReceiveSignal(rsignal.signal);
                    }
                }
            }));
        }

        public void ReceiveSignal(ISignal signal)
        {
            var type = signal.GetType();
            if (receivers.TryGetValue(type.ToString(),out var value)){
                value.ReceiveSignal(signal);
            }
        }

        private void Register<T>(ISignalReceiver receiver) {
            receivers[typeof(T).ToString()] = receiver;
        }

        private void Deregister<T>(ISignalReceiver receiver)
        {
            receivers.Remove(typeof(T).ToString());
        }

        public class RegisterSignalReceiverSignal {
            public ISignalReceiver receiver;
            public Type type;

            public RegisterSignalReceiverSignal(ISignalReceiver receiver, Type type) {
                this.receiver = receiver;
                this.type = type;
            }
        }

        public class DeregisterSignalReceiverSignal
        {
            public Type type;

            public DeregisterSignalReceiverSignal(ISignalReceiver receiver, Type type)
            {
                this.type = type;
            }
        }

        public class InvokeReceiverSignal {
            public string invokation;
            public ISignal signal;
            public InvokeReceiverSignal(string invokation, ISignal signal) {
                this.invokation = invokation;
                this.signal = signal;
            }
        }
    }
}