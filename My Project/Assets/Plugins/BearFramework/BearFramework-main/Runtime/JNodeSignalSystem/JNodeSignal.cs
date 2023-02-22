using Newtonsoft.Json.Linq;

namespace Bear
{


    public struct JNodeSignal : INodeSignal
    {
        public JToken Token { get; set; }
    }

    public static class JNodeSignalSystem {
        public static void Is<T>(this JNodeSignal signal,string key,T value) 
        {
            if (value is JToken jValue) {
                signal.Token[key]=jValue;
            }
            signal.Token[key] = new JValue(value);
        }

        public static void IsArray<T>(this JNodeSignal signal, string key, params T[] values)
        {
            var value = new JArray(values);
            signal.Token[key] = value;
        }

        
    }
}