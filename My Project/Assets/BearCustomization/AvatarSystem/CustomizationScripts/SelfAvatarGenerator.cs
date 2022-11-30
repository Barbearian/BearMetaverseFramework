
using UnityEngine;

namespace Bear
{
    [RequireComponent(typeof(LocalBearFramework),typeof(AvatarGenerator),typeof(BoneManager))]
    public class SelfAvatarGenerator : MonoBehaviour,ILocalFrameworkFetcher,IGlobalMailSender,IServiceFetcher
    {
        public IFrameworkFetcher GetFetcher()
        {
            if (_fetcher==null) {
                _fetcher = new FrameworkFetcher(gameObject);
            }
            return _fetcher;
        }

        public IFrameworkFetcher _fetcher;

        private void Awake()
        {
            GenerateSelfAvatar();
        }

        public void GenerateSelfAvatar() {
            if (GetFetcher().TryGetFramework(out var framework) && this.TryGetService<AvatarDataService>(out var service)) {
                this.SendGlobalMail(new ApplyMultiAvatarChangeSignal() { 
                    receiver = framework,
                    isLow = false,
                    code = service.GetMyModifiers()
                });
            }
        }
    }

    

    

    
}