using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class EquipmentMaker : MonoBehaviour
    {
        public Collider view;
        public GameObject Equipment;
        public string key;
        public DynamicGraphData data;
        public void Awake()
        {
            #region Data
            //Input->ItemInputSignalTranslatorNodeData->ItemStateMachine
            var map = new TypeSignalTranslator();
            var intKey = new IntKeyTransitionSignal()
            {
                Value = 0,
            };
            var ItemStatemachineSignal = new ItemStateMachineSignal()
            {
                signal = intKey,
            };
            map.signalMap[typeof(LBTPerformedSignal).ToString()] = ItemStatemachineSignal;


            //ItemStateMachine()->SignalContainer

            //actionContainer 

            var attackA = new SignalContainerTransferSignal() {
                key = "AttackA"
            };

            var playAnimSignal = new PlayAnimationNodeSignal()
            {
                info = new PlayAnimationClipInfo()
                {
                    clipName = "Pre_RightHand@Attack01",
                    layer = 1,

                }
            };

            var ItemActExSignal = new ItemActExSignal()
            {
                signals = new List<INodeSignal>(){ 
                    playAnimSignal,
                    new WielderToItemSignal(){
                        signal = new ItemStateEnterSignal(){
                            index = 1
                        }
                    },
                    new FaceInputDirSignal(){ 
                        moveAmount = 1
                    },

                }.ToArray(),
            };

            var attackB = new SignalContainerTransferSignal()
            {
                key = "AttackB"
            };


            var playAnimSignalB = new PlayAnimationNodeSignal()
            {
                info = new PlayAnimationClipInfo()
                {
                    clipName = "RightHand@Attack02",
                    layer = 1,

                }
            };

            var ItemActExSignalB = new ItemActExSignal()
            {
                signals = new List<INodeSignal>(){
                    playAnimSignalB,
                    new WielderToItemSignal(){ 
                        signal = new ItemStateEnterSignal(){ 
                            index = 2
                        }
                    },
                   

                }.ToArray(),
            };




            //Signal Container -> ItemToWielder
            #endregion

            var pickView = Instantiate(view).gameObject.AddComponent<NodeView>();
            pickView.AddNodeData(new PickUpViewNodeData(new InstanceFetcher(Equipment)));
            pickView.AddNodeData(new ItemPickUpNodeData());

            var equipmentView = new CNode();
            equipmentView.AddNodeData(new ItemNodeData());
            equipmentView.AddNodeData(new EquipmentModelNodeData() { 
                fetcher = new InstanceFetcher(Equipment),
                key = key
            });


            #region Input2Item

            equipmentView.AddNodeData(new ItemInputSignalTranslatorNodeData() { 
                translator = map
            });
            #endregion

            #region ItemStateMachine
            //Item State machine
            equipmentView.AddNodeData(new ItemStateMachineNodeData());
            
            //StateMachine
            var stateMachineNodeData = data.ToNodeData();
            var sm = equipmentView.GetOrCreateNodeData<DynamicStateMachineNodeData>();
            sm.Init(stateMachineNodeData.StateMachine);
            #endregion

            #region SignalContainer
            //Singal Container
            var container = new SignalContainerNodeData();
            equipmentView.AddNodeData(container);
            container.Register(attackA, ItemActExSignal);
            container.Register(attackB, ItemActExSignalB);

            #endregion

            #region ItemToWeilder
            equipmentView.AddNodeData(new ItemToWielderNodeData());
            #endregion


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