using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bear
{
    //[RequireComponent(typeof(Animator))]
    public class BoneManager : MonoBehaviour, ILocalFrameworkFetcher, ISignalReceiver
    {
        
        public UnityEvent DOnBoneMade;
        Dictionary<string, Transform> _boneDic;
        List<OnResourceMadeSignal> buffers = new List<OnResourceMadeSignal>();

        Dictionary<string, Transform> BoneDic {
            get {
                if ((!_inited) && _boneDic == null) {
                    _boneDic = new Dictionary<string, Transform>();
                    
                }
                return _boneDic;
            }
        }

        bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        bool _inited;
        public void Init()
        {
            _inited = false;            
            _boneDic = new Dictionary<string, Transform>();
            AddBone(transform);
            DOnBoneMade.Invoke();
            _inited = true;
            foreach (var item in buffers)
            {
                ReceiveSignal(item);
            }
            buffers.Clear();
            // Debug.Log("I am inited");
        }
        public bool TryGetBone(string boneKey, out Transform bone) {
            return BoneDic.TryGetValue(boneKey,out bone);
        }

        private void AddBone(Transform target)
        {
            if (target == null) return;
            _boneDic[target.name] = target;

            for (int i = 0; i < target.childCount; i++)
            {
                AddBone(target.GetChild(i));
            }
        }

        public void Awake()
        {
            this.RegisterMailBox(AvatarMakingKeywords.OnResourceMade,this);
        }

        IFrameworkFetcher _fetcher;
        public IFrameworkFetcher GetFetcher()
        {
            if (_fetcher == null) {
                _fetcher = new FrameworkFetcher(gameObject);
            }
            return _fetcher;
        }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is OnResourceMadeSignal ormsignal)
            {
                if (ormsignal.typeName == BodyPartKeywords.Body)
                {
                    Init();

                }
                else if (!_inited)
                {
                    buffers.Add(ormsignal);
                }
                else if (ormsignal.resource.TryGetComponent<SkinnedMeshRenderer>(out var smr))
                {
                    if (_inited)
                        Stitch(smr);
                }
                else if (ormsignal.resource.TryGetComponent<SkinnedMeshContainer>(out var smrc))
                {
                    if (_inited)
                    {
                        foreach (var item in smrc.GetSkinnedMeshes())
                        {
                            Stitch(item);
                        }
                    }
                }
            }
           
        }

        public void Stitch(SkinnedMeshRenderer smr) {
            smr.gameObject.layer = gameObject.layer;
            var bones = new Transform[smr.bones.Length];
            smr.rootBone = _boneDic[smr.rootBone.name];
            for (int i = 0; i < bones.Length; i++)
            {
                var mimic = smr.bones[i];

                bones[i] = _boneDic[mimic.name];
              //  Debug.Log(mimic.name);
            }
            smr.bones = bones;
        }

        
    }
}