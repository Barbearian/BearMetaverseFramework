using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bear
{
    public class BearFramework:IBearFramework,ISignalReceiver
    {
        Dictionary<string, IBearService> services = new Dictionary<string, IBearService>();
        Dictionary<string, BufferService> bufferedService = new Dictionary<string, BufferService>();
        //Dictionary<string, System.Action> _eventCenter = new Dictionary<string, System.Action>();
        public ISignalReceiver receiver;
        public UnityEvent InitEvent = new UnityEvent();

        bool Inited;

        bool _isActive;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void Init()
        {
            if (!Inited)
            {
                InitEvent.Invoke();
                Inited = true;

            }
        }
        public void DeregisterService(string key)
        {
            //   Init();
            services.Remove(key);
        }

        public void RegisterService(string serviceName, IBearService service)
        {
            //  Init();
            services[serviceName] = service;
            service.Init(this);

            if (bufferedService.TryGetValue(serviceName,out var buffer)) {
                buffer.Registered(service);
            }
        }

        public bool TryGetService<T>(string serviceName, out T rs) where T : IBearService
        {
            Init();
            if (services.TryGetValue(serviceName, out var value))
            {
              //  Debug.Log($"{serviceName} is found");
                if (value is T targetService)
                {
                    rs = targetService;
                    return true;
                }
            }

            Debug.Log($"{serviceName} is missing");
            rs = default;
            return false;
        }

        public void ReceiveSignal(ISignal signal)
        {
            receiver?.ReceiveSignal(signal);
        }

        public void RequestService(string serviceName, Action<IBearService> DOnServiceFetched)
        {
            if (TryGetService<IBearService>(serviceName, out var Service))
            {
                DOnServiceFetched?.Invoke(Service);
            }
            else if (bufferedService.TryGetValue(serviceName, out var bService))
            {
                bService.ReceiveSignal(new ServiceRequestSignal(serviceName, DOnServiceFetched));
            }
            else {
                var buffered = new BufferService();
                bufferedService[serviceName] = buffered;
                buffered.ReceiveSignal(new ServiceRequestSignal(serviceName, DOnServiceFetched));
            }
        }

        public BearFramework(ISignalReceiver receiver) {
            this.receiver = receiver;
        }

        
    }
}