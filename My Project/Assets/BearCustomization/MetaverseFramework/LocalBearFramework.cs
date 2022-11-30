using System;
using UnityEngine;

namespace Bear
{
    public class LocalBearFramework : MonoBehaviour, IBearFramework
    {
        public BearFramework bf = new BearFramework(new MailBox());
        public bool IsActive { get => bf.IsActive; set => bf.IsActive = value; }

        public void DeregisterService(string key)
        {
            bf.DeregisterService(key);
        }

        public void ReceiveSignal(ISignal signal)
        {
            bf.ReceiveSignal(signal);
        }

        public void RegisterService(string serviceName, IBearService service)
        {
            bf.RegisterService(serviceName,service);
        }

        public void RequestService(string serviceName, System.Action<IBearService> DOnServiceFetched)
        {
            bf.RequestService( serviceName, DOnServiceFetched);
        }

        public bool TryGetService<T>(string serviceName, out T rs) where T : IBearService
        {
            return bf.TryGetService<T>(serviceName,out rs);
        }

        
    }
}