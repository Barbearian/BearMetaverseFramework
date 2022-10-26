using System.Collections.Generic;

namespace Bear
{

    
    public interface IBearFramework:ISignalReceiver{
        bool TryGetService<T>(string serviceName,out T rs) where T: IBearService;
        void RegisterService(string serviceName, IBearService service);
        void DeregisterService(string key);

        void RequestService(string serviceName,System.Action<IBearService> DOnServiceFetched);
        //bool TryGetBufferService(string serviceName,out BufferService buffer);
    }

    //Service is for finite amount only
    public interface IBearService {
        void Init(IBearFramework framewrok);
    }

    public interface IServiceFetcher {}
    public interface IServiceRequester<T> 
        where T:IBearService
    {
        IServiceRquestHelper<T> serviceHelper { get; }
    }
    public interface IServiceRquestHelper<T>
        where T : IBearService
    {
        void Init(IBearService service);
        bool TryGetService(out T service);
    }

    public class ServiceRequestHelper<T> : IServiceRquestHelper<T>, IServiceRequester<T>
        where T:IBearService
    {
       // bool requested;
        bool inited;
        T service;

        public IServiceRquestHelper<T> serviceHelper => this;

        public void Init(IBearService service)
        {
            if (service is T requestedService) {
                this.service = requestedService;
                inited = true;
                
              //  requested = true;
            }
        }

        public bool TryGetService(out T service)
        {
            if (inited) {
                service = this.service;
                return true;
            }
            service = default;
            return false;
        }
    }

    public interface ISignalSender { }
    
    public interface IFrameworkInjector {
        //void Init();
        IBearFramework GetFramework();
    }

    public interface IServiceRegister { }
    
    

    public static class BearFrameworkHandler {
        public static IBearFramework framework;
        public static bool _inited;
        public static bool TryGetService<T>(this IServiceFetcher fetcher,string code,out T service)
            where T : IBearService
        {
            if (_inited)
            {
                return framework.TryGetService<T>(code, out service);
            }
            else {
                service = default;
                return false;
            }
        }

        public static bool TryGetService<T>(this IServiceFetcher fetcher, out T service)
            where T : IBearService
        {
            return fetcher.TryGetService<T>(typeof(T).ToString(),out service);
        }

        public static bool TryGetService<T>(this IBearFramework fetcher, out T service)
            where T : IBearService
        {
            return fetcher.TryGetService<T>(typeof(T).ToString(), out service);
        }

        public static void InjectFramework(this IFrameworkInjector injector) {
            framework = injector.GetFramework();
            _inited = true;
        }

        public static void SendSignal(this ISignalSender sender, ISignal signal) {
            framework?.ReceiveSignal(signal);
        }

        public static void RegisterService(this IServiceRegister register, string ServiceName, IBearService service) {
            framework?.RegisterService(ServiceName, service);
        }

        public static void RegisterService(this IServiceRegister register,  IBearService service)
        {
            framework?.RegisterService(service.GetType().ToString(), service);
        }

        public static void RegisterService(this IBearFramework framework,IBearService bearService) {
            framework.RegisterService(bearService.GetType().ToString(),bearService);
        }

        public static void RequestService<T>(this IServiceRequester<T> requester,string ServiceName) 
            where T:IBearService
        {
            framework.RequestService(ServiceName,requester.serviceHelper.Init);
        }
    }

    public class ServiceRequestSignal:ISignal {
     //   public IBearService service;
        public string serviceName;
        public System.Action<IBearService> DOnServiceFetched;

        public ServiceRequestSignal(string serviceName, System.Action<IBearService> DOnServiceFetched)
        {
            this.serviceName = serviceName;
            this.DOnServiceFetched = DOnServiceFetched;
        }
    }

    public class BufferService : IBearService,ISignalReceiver
    {
        public List<ServiceRequestSignal> services = new List<ServiceRequestSignal>();

        public bool IsActive { get => true; set { } }

        public void Init(IBearFramework framewrok)
        {
           // throw new System.NotImplementedException();
        }

        void AddService(ServiceRequestSignal signal) {
            services.Add(signal);
        }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is ServiceRequestSignal srsignal) {
                AddService(srsignal);
            }
        }

        public void Registered(IBearService service) {
            foreach (var item in services)
            {
                item.DOnServiceFetched?.Invoke(service);
            }
            services.Clear();
        }
    }

}