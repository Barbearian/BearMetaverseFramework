using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    [RequireComponent(typeof(ISignalReceiver))]
    public class AvatarSignalSender : MonoBehaviour, IGlobalMailSender
    {
        public string[] RefreshSignals;
        ISignalReceiver _receiver;
        ISignalReceiver Receiver {
            get {
                if (_receiver==null) {
                    _receiver = GetComponent<ISignalReceiver>();
                }
                return _receiver; }
            
        }
        

        public void Send(string[] signals) {
            foreach (var item in signals)
            {
                this.SendGlobalMail(typeof(AvatarCustomizationService).ToString(),new ApplyAvatarChangeSignal() { 
                    code = item,
                    receiver = Receiver
                });
            }
        }

        public void Send() {
            Send(RefreshSignals);
        }

        IFrameworkFetcher fetcher;
        public IFrameworkFetcher GetFetcher()
        {
            if (fetcher == null) {
                fetcher = new FrameworkFetcher(gameObject);
            }

            return fetcher;
        }

    }

    [System.Serializable]
    public class TestTuple{
        public string Item1;
        public string[] Item2;
    }
}