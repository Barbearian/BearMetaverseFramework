using UnityEngine;

namespace Bear
{
    public class EquipmentModelNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
    {
        private ArrayNodeSignalReceiver receivers = new ArrayNodeSignalReceiver();
        public IFetcher<GameObject> EquipmentFetcher;
        public string tag;
        public NodeView Wielder { get; set; }

        public EquipmentModelNodeData(IFetcher<GameObject> EquipmentFetcher) { 
            this.EquipmentFetcher = EquipmentFetcher;
        }

        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<OnEquipSignal>((x) => {
                Wielder = x.eData.Wielder;
                this.Show();
            },true).AddTo(receivers);

           
        }

        public void Detached(INode node)
        {
            receivers.InhibitAll();
        }
    }

    public static class EquipmentModelNodeDataSystem
    {
        public static void Show(this EquipmentModelNodeData data) { 
            var finder = data.Wielder.GetOrCreateNodeData(new FinderNodeData());
            if (finder.TryFind(data.tag,out var anchor)) {
                var model = data.EquipmentFetcher.GetValue();
                model.transform.parent = anchor.transform;

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;   

                model.SetActive(true);

            }   
        }

        public static void Hind(this EquipmentModelNodeData data)
        {
            data.EquipmentFetcher.GetValue().SetActive(false);
        }


        public static EquipmentModelNodeData CreateStandardEquipmentVisualizeNodeData(GameObject prefab,string tag) {
            var rs = new EquipmentModelNodeData(new InstanceFetcher(prefab));
            rs.tag= tag;
            return rs;
        }
    }

  

    

}