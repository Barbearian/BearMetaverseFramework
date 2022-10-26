using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class AvatarGenerateData
    {

        Dictionary<(string, string), ResourceModifySignal> ResourceModifySignals = new Dictionary<(string, string), ResourceModifySignal>();
        Dictionary<string, ResourceMakeSignal> ResourceMakeSignals = new Dictionary<string, ResourceMakeSignal>();

        public void UpdateResourceModifySignal(ResourceModifySignal rmsignal)
        {
            ResourceModifySignals[(rmsignal.ResourceKey, rmsignal.SubModificationKey)] = rmsignal;
            
        }

        

        public void UpdateResourceMakeSignal(ResourceMakeSignal resourceMakeSignal) {
            ResourceMakeSignals[resourceMakeSignal.typeName] = resourceMakeSignal;
            
        }

        public IEnumerable<ResourceModifySignal> GetAffected(string key) {
            List<ResourceModifySignal> rs = new List<ResourceModifySignal>();
            foreach (var item in ResourceModifySignals)
            {
                if (item.Key.Item1.Equals(key)) {
                    rs.Add(item.Value);
                }
            }
            return rs;
        }

        public string[] GetAllSignals() {
            List<string> rs = new List<string>();
            foreach (var item in ResourceModifySignals)
            {
                rs.Add(item.Value.id);
            }

            foreach (var item in ResourceMakeSignals)
            {
                rs.Add(item.Value.id);
            }
            return rs.ToArray();
        }

        public void Make(ISignalReceiver receiver) {
            foreach (var item in ResourceModifySignals)
            {
                receiver.ReceiveMail(item.Value);
            }

            foreach (var item in ResourceMakeSignals)
            {
                receiver.ReceiveMail(item.Value);
            }
        }

        

    }
}