using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class AvatarGenerator : MonoBehaviour,IServiceFetcher,ILocalFrameworkFetcher,ISignalReceiver,IResourceContainer
    {
        public Dictionary<string, SkinnedMeshRenderer[]> smrs = new Dictionary<string, SkinnedMeshRenderer[]>();
        public Dictionary<string, GameObject> refs = new Dictionary<string, GameObject>();

        public AvatarGenerateData agd = new AvatarGenerateData();

        public Dictionary<string, GameObject> Resources => refs;

        IFrameworkFetcher fetcher;

        bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        

        public IFrameworkFetcher GetFetcher()
        {
            if (fetcher == null)
            {
                fetcher = new FrameworkFetcher(gameObject);
            }
            return fetcher;
        }

        private void Awake()
        {
            this.RegisterMailBox(AvatarMakingKeywords.ResourceMaker,this);
            this.RegisterMailBox(typeof(RequstLocalAvatarDataSignal).ToString(),new ActionSignalReceiver((signal) => {
                if (signal is RequstLocalAvatarDataSignal rsignal) {
                    rsignal.DonReceivedData(agd);
                }
            }));

           
        }

       

        public void Make(string key,string reference) {

            // AddressableAssetSettingsDefaultObject.Settings.GetLabels();
            if (this.TryGetService<IResourceManager>(out var service)) {
                service.Load<GameObject>(reference, (x) => {
                    //Debug.Log(reference);
                    Release(key);
                    var obj = Instantiate(x, transform);
                    OnMake(key,obj);
                });
            }
           
        }

        public void Make(string key, string[] references) {
            if (references.Length == 1)
            {
                Make(key,references[0]);
                return;
            }

            if (this.TryGetService<IResourceManager>(out var service))
            {
               
                var container = new GameObject().AddComponent<SkinnedMeshContainer>();
                container.name = key;
                container.renderers = new SkinnedMeshRenderer[references.Length];
                container.transform.SetParent(transform);
                AsyncCounter counter = new AsyncCounter(references.Length, () => {
                    Release(key);
                    OnMake(key, container.gameObject);
                });

                
                for (int i = 0; i < references.Length; i++)
                {
                    var reference = references[i];
                    int currentCount = i;
                    service.Load<GameObject>(reference, (x) => {
                        
                        var obj = Instantiate(x, container.transform).GetComponent<SkinnedMeshRenderer>();
                        container.GetSkinnedMeshes()[currentCount] = obj;
                        counter.Tick();
                    });
                }

                
                
            }
        }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is ResourceMakeSignal rmsignal)
            {
                agd.UpdateResourceMakeSignal(rmsignal);
                Make(rmsignal.typeName, rmsignal.resourceName);
            }
            else if (signal is CombinedModifierSignal cSignal)
            {
                cSignal.ApplySignal(ReceiveSignal);
                //UpdateRMS(cSignal);
            }
            else if (signal is ResourceModifySignal rmdiSignal)
            {
                OnReceiveModifierSignal(rmdiSignal);
                agd.UpdateResourceModifySignal(rmdiSignal);

            }
        }

        void OnReceiveModifierSignal(ResourceModifySignal rmdiSignal) {
            if (rmdiSignal is CombinedModifierSignal csignal) {
                ReceiveSignal(csignal);
                return; 
            }
            if (smrs.TryGetValue(rmdiSignal.ResourceKey, out var smrList))
            {
                int index = rmdiSignal.SmrIndex;
                if (index >= 0 && index < smrList.Length)
                {
                    rmdiSignal.Modify(smrList[index]);

                }


            }
            
            
            
            
        }

        //public void UpdateRMS(ResourceModifySignal rmsignal) {
        //    rms[(rmsignal.ResourceKey, rmsignal.SubModificationKey)] = rmsignal;
        //}

        public void OnMake(string keyword, GameObject obj)
        {
            refs[keyword] = obj;
            if (obj.TryGetComponent<SkinnedMeshRenderer>(out var smr)) {
                smrs[keyword] = new SkinnedMeshRenderer[] { smr};
            } else if (obj.TryGetComponent<ISkinnedMeshContainer>(out var container)) {
                smrs[keyword] = container.GetSkinnedMeshes();
            }


            ApplyModifiers(keyword);
            
            this.SendMail(AvatarMakingKeywords.OnResourceMade,new OnResourceMadeSignal() { typeName = keyword,resource = obj});

            if (keyword.Equals( BodyPartKeywords.Body)) {
                foreach (var item in refs)
                {
                    if (!item.Equals(BodyPartKeywords.Body)) {
                        this.SendMail(AvatarMakingKeywords.OnResourceMade, new OnResourceMadeSignal() { typeName = item.Key, resource = item.Value });
                    }
                    
                }
            }
        }

        

        public void Release(string keyword) {
            if (refs.TryGetValue(keyword,out var value)) {
               // Debug.Log(keyword);
                
                Destroy(value);

            }
            smrs.Remove(keyword);
            refs.Remove(keyword);
        }

        void ApplyModifiers(string key)
        {
            foreach (var item in agd.GetAffected(key))
            {
                OnReceiveModifierSignal(item);
                
            }

        }
    }

    public class RequstLocalAvatarDataSignal :ISignal{
        public System.Action<AvatarGenerateData> DonReceivedData;
        public RequstLocalAvatarDataSignal(System.Action<AvatarGenerateData> DonReceivedData) {
            this.DonReceivedData = DonReceivedData;
        }
    }
    
}