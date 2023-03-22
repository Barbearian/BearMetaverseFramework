

using Newtonsoft.Json.Linq;

namespace Bear
{
    public class JNodeSignalTransferNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        private INode root;
        public void Attached(INode node)
        {
            root = node;
            node.RegisterSignalReceiver<JNodeSignal>(ProcessData,true).AddTo(receivers);
        }

        public void ProcessData(JNodeSignal signal) {
            var address = signal.PopTransferAddress();
            if (!string.IsNullOrEmpty(address))
            {
                root.ReceiveNodeSignal(address, signal);
            }

        }
    }

    public static class JNodeSignalTransferNodeDataTool {
        public static void AppendTransferAddress(this JNodeSignal signal,string address) {
            var token = signal.Token[typeof(JNodeSignalTransferNodeData).ToString()];
            if (token is JArray list)
            {
                list.Add(address);
            }
        }

        public static void AssignTransferAddress(this JNodeSignal signal,string address) {
            var key = typeof(JNodeSignalTransferNodeData).ToString();
            signal.Token[key] = address;
        }

        public static string PopTransferAddress(this JNodeSignal signal)
        {
            var token = signal.Token[typeof(JNodeSignalTransferNodeData).ToString()];

            if (token is JArray list)
            {
                if (list.Count >= 1)
                {
                    var addressToken = list[0];
                    list.RemoveAt(0);
                    var address = addressToken.ToObject<string>();

                    return address;

                }
            }

            if (token is JValue value)
            {
                var address = value.ToObject<string>();
                return address;
            }
            return null;
        }
    }
}