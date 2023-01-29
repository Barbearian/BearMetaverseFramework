using System.Collections.Generic;

namespace Bear
{
    public class SignalTransferNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
    {
        private bool Inited = false;
        public INode input { get; set; }
        public INode output { get; set; }
        private Dictionary<string, ActionNodeSignalReceiver> buffer { get; set; } = new Dictionary<string, ActionNodeSignalReceiver>();

        public void Attached(INode node)
        {
            output = node;
        }

        public void Init(INode input) { 
            this.input = input; 
            Inited= true;
        }

        public bool Register(string key,INodeSignal transfered) {
            if (Inited) {
                Deregister(key);
                ActionNodeSignalReceiver action = new ActionNodeSignalReceiver((x) => { 
                    output.ReceiveNodeSignal(x);
                });
                input.RegisterSignalReceiver(key,action,true);
                buffer[key] = action;
                return true;
            }

            return false;
        }

        public bool Register<T>(INodeSignal transfered)
        {
            var key = typeof(T).ToString();
            return Register(key, transfered);
        }

        public void Deregister(string key,bool removeKey = true) {
            if (buffer.TryGetValue(key,out var action)) {
                action.SetActive(false);
                if (removeKey)
                    buffer.Remove(key);
            }
        }

        public void Deregister<T>(bool removeKey = true)
        {
            Deregister(typeof(T).ToString());
        }

        public void DeregistrAll() {
            foreach (var key in buffer.Keys)
            {
                Deregister(key,false);
            }

            buffer = new Dictionary<string, ActionNodeSignalReceiver>();
        }

        public void Detached(INode node)
        {
            DeregistrAll();
        }
    }
}