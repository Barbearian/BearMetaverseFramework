using UnityEngine;

namespace Bear
{

    public class VFXNodeData : INodeData, IOnAttachedToNode
    {
        private ArrayNodeSignalReceiver receivers = new ArrayNodeSignalReceiver();
        public IVFXMaker maker;
        public VFXNodeData(IVFXMaker maker) { 
            this.maker = maker;
        }
        public void Attached(INode node)
        {
            Init(node);
        }

        private void Init(INode node) {
            node.RegisterSignalReceiver<ActSignal>((x) => {
                
            }, true).AddTo(receivers);
        }
    }

    public interface IVFXMaker {
        public T GetVFX<T>() where T:Component;
    }

    public class NaiveVFXMaker : IVFXMaker
    {
        public GameObject Prefab;
        public T GetVFX<T>() where T : Component
        {
            var rs = Object.Instantiate(Prefab) as T;
            rs.gameObject.SetActive(false);

            return rs;
        }
    }

}