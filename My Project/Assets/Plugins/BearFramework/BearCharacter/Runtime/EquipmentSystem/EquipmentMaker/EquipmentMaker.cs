using UnityEngine;

namespace Bear
{
    public class EquipmentMaker : MonoBehaviour
    {
        public Collider view;
        public GameObject Equipment;
        public void Awake()
        {
            var pickView = Instantiate(view).gameObject.AddComponent<NodeView>();
            pickView.AddNodeData(new PickUpViewNodeData(new InstanceFetcher(Equipment)));
            pickView.AddNodeData(new ItemPickUpNodeData());
            pickView.transform.position = this.transform.position;
        }
    }
}