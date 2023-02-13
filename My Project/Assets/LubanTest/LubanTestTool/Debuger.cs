
using cfg;
using SimpleJSON;
using System.IO;
using UnityEngine;

namespace Bear
{
    public class Debuger : MonoBehaviour
    {
        private void Awake()
        {
            Log();
        }

        [ContextMenu("Log")]
        public void Log() {
            Tables table = new Tables(Loader);
            CanSignal signal = table.TbCanSignals.Get(1001); 
            Debug.Log(signal);
        }

        private JSONNode Loader(string fileName) {
            return JSON.Parse(File.ReadAllText(Application.dataPath +"/GenerateData/json/"+fileName+".json"));
        }
    }
}