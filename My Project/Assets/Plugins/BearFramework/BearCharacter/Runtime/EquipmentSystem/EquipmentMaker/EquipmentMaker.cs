using UnityEngine;

namespace Bear
{
    public class EquipmentMaker : MonoBehaviour
    {
        public Collider view;
        public string equipmentKey;
        public GameObject Equipment;
        public void Awake()
        {
            var pickView = Instantiate(view).gameObject.AddComponent<NodeView>();
            pickView.AddNodeData(new PickUpViewNodeData(new InstanceFetcher(Equipment)));
            pickView.AddNodeData(new ItemPickUpNodeData());

            var equipmentView = new GameObject(Equipment.name).AddComponent<NodeView>();
            equipmentView.AddNodeData(new EquipmentNodeData());
            equipmentView.AddNodeData(new EquipmentModelNodeData() { 
                fetcher = new InstanceFetcher(Equipment)
            });

            pickView.AddNodeData(new EquipmentTestorNodeData() { 
                signal = new EquipSignal() { 
                    equipment= equipmentView,
                    key= equipmentKey,
                }
            });


            pickView.transform.position = this.transform.position;
        }
    }
}