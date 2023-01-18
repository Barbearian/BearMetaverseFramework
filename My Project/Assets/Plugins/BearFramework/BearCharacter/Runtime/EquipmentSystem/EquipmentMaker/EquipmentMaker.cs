using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class EquipmentMaker : MonoBehaviour
    {
        public GameObject Equipment;
        public void Awake()
        {
            var pickView = new GameObject("Equipment").AddComponent<NodeView>();
            //pickView.AddNodeData(new PickUpViewNodeData());
            
        }
    }
}