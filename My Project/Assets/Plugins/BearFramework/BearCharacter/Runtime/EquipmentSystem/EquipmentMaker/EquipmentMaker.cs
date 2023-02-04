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

            var equipmentView = new CNode();
            equipmentView.AddNodeData(new ItemNodeData());
            equipmentView.AddNodeData(new EquipmentModelNodeData() { 
                fetcher = new InstanceFetcher(Equipment),
                key = key
            });
            equipmentView.AddNodeData(new ItemStateMachineNodeData());

            var map = new TypeSignalTranslator();

            var playAnimSignal = new PlayAnimationNodeSignal()
            {
                info = new PlayAnimationClipInfo()
                {
                    clipName = "RightHand@Attack01",
                    layer = 1,

                }
            };
            var ItemActExSignal = new ItemActExSignal()
            {
                signal = playAnimSignal,
            };
            var ItemStatemachineSignal = new ItemStateMachineSignal() { 
                signal = ItemActExSignal,
            };
            map.signalMap[typeof(LBTPerformedSignal).ToString()] = ItemStatemachineSignal;
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