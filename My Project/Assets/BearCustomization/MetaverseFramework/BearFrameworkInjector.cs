
using UnityEngine;
namespace Bear
{
    public class BearFrameworkInjector : IFrameworkInjector
    {
        public IBearFramework framework = new BearFramework(new MailBox());


        public void RegisterServices() {
            framework.RegisterService(typeof(IResourceManager).ToString(), new ResourceManagerService());
            framework.RegisterService(new AvatarCustomizationService());
            framework.RegisterService(new AvatarDataService());
        }



        public IBearFramework GetFramework()
        {
            
            return framework;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init() {
            var injector = new BearFrameworkInjector();
            injector.InjectFramework();
            injector.RegisterServices();
        }
    }
}