using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class AvatarDataService : IBearService,IServiceFetcher,IGlobalMailSender,IGlobalMailRegister
    {
        public string[] myData;
        public int bodyType =-1;
        public void Init(IBearFramework framewrok)
        {
            this.RegisterGlobalMailBox("FailedSynceSelfAvatarSignal", new Neuron());
            this.RegisterGlobalMailBox("PushedSelfAatarSignal", new Neuron());
            this.RegisterGlobalMailBox<UpdateAvatarDataSignal>(new ActionSignalReceiver((signal) => {
                if (signal is UpdateAvatarDataSignal uads) {
                    myData = uads.myData;
                    bodyType = uads.bodyType;
                }
            }));
        }

        public string[] GetMyModifiers() {
            return myData;
        }

        public void Init(string[] allModifiers,System.Action DOnInited) {
            
        }       

        public void Update(string[] data,int bodyType) {
            myData = data;
            this.bodyType = bodyType;
        }

    }

    public class FailedSynceSelfAvatarSignal : ISignal {}
    public class PushedSelfAatarSignal : ISignal { }

    public class UpdateAvatarDataSignal : ISignal {
        public string[] myData;
        public int bodyType;
    }
}