using UnityEditor;
using UnityEngine;

namespace Bear
{
    public class EquipmentMaker : MonoBehaviour
    {
        public Collider view;
        public GameObject Equipment;
        public string key;
        public void Awake()
        {
            var pickView = Instantiate(view).gameObject.AddComponent<NodeView>();
            pickView.AddNodeData(new PickUpViewNodeData(new InstanceFetcher(Equipment)));
            pickView.AddNodeData(new ItemPickUpNodeData());

            var equipmentView = new GameObject(Equipment.name).AddComponent<NodeView>();
            equipmentView.AddNodeData(new EquipmentNodeData());
            equipmentView.AddNodeData(new EquipmentModelNodeData() { 
                fetcher = new InstanceFetcher(Equipment),
                key = key
            });
            var map = new TypeSignalTranslator();
            map.signalMap[typeof(LBTPerformedSignal).ToString()] = new PlayAnimationNodeSignal() {
                info = new PlayAnimationClipInfo() { 
                    clipName= "RightHand@Attack01",
                    layer = 1,
                    
                }
            };
            equipmentView.AddNodeData(new ItemInputSignalTranslatorNodeData() { 
                translator = map
            });




            pickView.AddNodeData(new EquipmentTestorNodeData() { 
                signal = new EquipSignal() { 
                    equipment= equipmentView,
                }
            });


            pickView.transform.position = this.transform.position;


        }

        [ContextMenu("Do Something")]
        public void Test() {
            var node = new CNode();
            node.RegisterSignalReceiver<RollActivateSignal>((x) => {
                Debug.Log("乐");
            });

            node.ReceiveNodeSignal(new RollActivateSignal());  
        }
    }
}