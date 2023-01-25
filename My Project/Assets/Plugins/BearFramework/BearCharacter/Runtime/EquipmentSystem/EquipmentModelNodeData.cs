using UnityEngine;

namespace Bear
{
    public class EquipmentModelNodeData : SignalHandlerNodeData,IOnAttachedToNode
    {
        public IFetcher<GameObject> fetcher;    
        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<OnEquippedSignal>((x) => {
                this.ShowEquipment(x.equipmentKey,x.wielder);
            },true).AddTo(receivers);

            node.RegisterSignalReceiver<OnUnequippedSignal>((x) => {
                if (node is NodeView view) { 
                    Object.Destroy(fetcher.GetValue().gameObject);    
                }
            }, true).AddTo(receivers);
        }

    }

    public static class EquipmentModelNodeDataSystem {
        public static void ShowEquipment(this EquipmentModelNodeData data, string tag,INode wielder) {
            Debug.Log("I tried to show data");
            if (wielder.TryGetNodeData<FinderNodeData>(out var finder))
            { 
                if(finder.TryFind(tag,out var view)){
                    view.AddChildrenAtZero(data.fetcher.GetValue().transform); 
                }
            }
        }
    }





  

    

}