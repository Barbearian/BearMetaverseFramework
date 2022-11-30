using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class LocalBearService : MonoBehaviour, IBearService
    {
        public virtual void Init(IBearFramework framework)
        {
            _framework = framework;
            _inited = true;
        }

        IBearFramework _framework;
        bool _inited;
        
        protected IBearFramework Framework {
            get {
                if (_inited)
                {
                    return _framework;
                }
                else {
                    Init();
                    return _framework;
                }
            }
        }

        public void Init()
        {
            if (!_inited)
            {
                _framework = GetComponentInParent<IBearFramework>();
                _inited = true;
            }
           // return _framework;
        }


    }

    public static class LocalFrameworkHandler {
        public static bool TryGetLocalFramework(this ILocalFrameworkFetcher fetcher, out IBearFramework framework) {
            return fetcher.GetFetcher().TryGetFramework(out framework);
        }

        public static void SendMail(this ILocalFrameworkFetcher fetcher,string address,ISignal mail) {
            if (fetcher.TryGetLocalFramework(out var framework)) {
                framework.ReceiveSignal(new DeliverSignal(address,mail));
            }
        }

        public static void SendMail(this ILocalFrameworkFetcher fetcher,  ISignal mail)
        {
            if (fetcher.TryGetLocalFramework(out var framework))
            {
                framework.ReceiveSignal(new DeliverSignal(mail.GetType().ToString(), mail));
            }
        }

        public static void RegisterMailBox(this ILocalFrameworkFetcher fetcher, string address, ISignalReceiver mailbox) {
            if (fetcher.TryGetLocalFramework(out var framework))
            {
                framework.ReceiveSignal(new RegisterSignal(address,mailbox));
            }
        }

        public static void RegisterMailBox<T>(this ILocalFrameworkFetcher fetcher,  ISignalReceiver mailbox)
        {
            if (fetcher.TryGetLocalFramework(out var framework))
            {
                framework.ReceiveSignal(new RegisterSignal(typeof(T).ToString(), mailbox));
            }
        }

        public static void RegisterMailBox<T>(this ILocalFrameworkFetcher fetcher, System.Action<ISignal> mailbox)
        {
            fetcher.RegisterMailBox<T>(new ActionSignalReceiver(mailbox));
           
        }

        public static void RequestService<T>(this ILocalFrameworkFetcher fetcher,IServiceRequester<T> request, string ServiceName) 
            where T:IBearService
        {
            if (fetcher.TryGetLocalFramework(out var framework)) {
                framework.RequestService(ServiceName,request.serviceHelper.Init);
            }
        }

        public static void RegisterLocalService<T>(this ILocalFrameworkFetcher fetcher,  string ServiceName, IBearService service)
            where T:IBearService
        {
            if (fetcher.TryGetLocalFramework(out var framework))
            {
                framework.RegisterService(ServiceName, service);
            }
        }

        public static void RegisterLocalService<T>(this ILocalFrameworkFetcher fetcher, IBearService service)
            where T : IBearService
        {
            fetcher.RegisterLocalService<T>(service.GetType().ToString(),service);
        }
    }

    public interface ILocalFrameworkFetcher {
        IFrameworkFetcher GetFetcher();
    }

    public interface IFrameworkFetcher {
        bool TryGetFramework(out IBearFramework framework);
    }

   

    public class FrameworkFetcher: IFrameworkFetcher
    {
        bool _inited;
        bool hasFramework;
        IBearFramework _framework;
        GameObject obj;
        public FrameworkFetcher(GameObject obj) {
            this.obj = obj;
        }
        public IBearFramework Framework
        {
            get
            {
                if (_inited)
                {
                    return _framework;
                }
                else
                {
                    Init();
                    return _framework;
                }
            }
        }

        public bool TryGetFramework(out IBearFramework framework) {
            if (hasFramework)
            {
                framework = _framework;
                return true;
            }
            else {
                framework = Framework;
                hasFramework = framework != null;           
                return hasFramework;
            }
        }
        

        public void Init()
        {
            if (!obj.TryGetComponent<IBearFramework>(out _framework))
            {
                _framework = obj.GetComponentInParent<IBearFramework>();
            }
            _inited = true;
            // return _framework;
        }
    }
}