
using System.Collections.Generic;
using UnityEngine;
namespace Bear
{
    public class DymanicNodeStateMachineTestor : MonoBehaviour
    {
        public DynamicGraphData graph;
        [ContextMenu("Show Graph")]
        public void ShowGraph() {
            Debug.Log(JsonUtility.ToJson(graph));

        }

        


        
    }

    
}