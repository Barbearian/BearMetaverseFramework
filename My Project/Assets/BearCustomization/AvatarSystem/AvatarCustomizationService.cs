using System.Collections.Generic;
using System.Linq;

namespace Bear
{
    public class AvatarCustomizationService : IBearService, IServiceFetcher, IGlobalMailRegister, ISignalReceiver
    {
        public Dictionary<string, ISignal> choices = new Dictionary<string, ISignal>();
        public Dictionary<string, ISignal> lowChoices = new Dictionary<string, ISignal>();
        public Dictionary<string, ChoiceGroup> groups = new Dictionary<string, ChoiceGroup>();
        bool _init;

        public bool IsActive { get => true; set { } }
        public bool TryGetSignal(string key,out ISignal signal) {
            return choices.TryGetValue(key,out signal);
        }
        public void Init(IBearFramework framewrok)
        {

            


        }

        private void CreateChoice(string ChoiceKey, ISignal value)
        {
            choices[ChoiceKey] = value;
        }


        public void ReceiveSignal(ISignal signal)
        {
            if (signal is RandomCustomizationSignal RCSignal)
            {
                var ChoiceGroup = groups.Values.ToArray().RandomValue();

                foreach (var item in ChoiceGroup.choiceByGroup.Values)
                {
                    var choice = item.ToArray().RandomValue();
                    if (choices.TryGetValue(choice, out var sig))
                    {
                        RCSignal.ReceiveMail(sig);
                    }
                }

            }
            else if(signal is ApplyAvatarChangeSignal aacsignal)
            {
                if (choices.TryGetValue(aacsignal.code, out var sig))
                {
                    aacsignal.ReceiveMail(sig);
                }
            }
        }

        public bool TryGetLow(string key,out ISignal signal) {
            if (lowChoices.TryGetValue(key, out signal))
            {
                return true;
            }
            else {
                return choices.TryGetValue(key,out signal);
            }
        }
    
    }

    public class RandomCustomizationSignal:ISignal {
        public ISignalReceiver receiver;
        public void ReceiveMail(ISignal signal) {
            receiver.ReceiveMail(AvatarMakingKeywords.ResourceMaker,signal);
        }
    }

    public class ApplyAvatarChangeSignal : ISignal {
        public ISignalReceiver receiver;
        public string code;
        public void ReceiveMail(ISignal signal)
        {
            receiver.ReceiveMail(AvatarMakingKeywords.ResourceMaker, signal);
        }
    }

    public class ApplyMultiAvatarChangeSignal : ISignal {
        public ISignalReceiver receiver;
        public string[] code;
        public bool isLow;
        public void ReceiveMail(ISignal signal)
        {
            receiver.ReceiveMail(AvatarMakingKeywords.ResourceMaker, signal);
        }
    }

    public class RequestAvatarSignal : ISignal {
        public System.Action<ISignal> DOnRequested;
        public System.Action DOnRequestFailed;
        public string request;

        public RequestAvatarSignal(string request, System.Action<ISignal> DOnRequested,System.Action DOnRequestedFailed
        ) {
            this.request = request;
            this.DOnRequested = DOnRequested;
            this.DOnRequestFailed = DOnRequestedFailed;
        }

        public RequestAvatarSignal(string request, System.Action<ISignal> DOnRequested
        )
        {
            this.request = request;
            this.DOnRequested = DOnRequested;
            this.DOnRequestFailed = () => { };
        }
    }

    public class ChoiceGroup {
        public Dictionary<string, List<string>> choiceByGroup = new Dictionary<string, List<string>>();
        public ChoiceGroup(Dictionary<string, List<string>> choiceByGroup) {
            this.choiceByGroup = choiceByGroup;
        }
    }

    public static class RandomHelper {
        public static V RandomValue<V>(this V[] value) {
            if (value.Length>0) {
                int index = UnityEngine.Random.Range(0,value.Length);
                return value[index];
            }
            else {
                return default;
            }
            
        }
    }

    
}