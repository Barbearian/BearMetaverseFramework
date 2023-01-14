
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bear {
    public class AnimatorProcessorNodeData : INodeData, IOnAttachedToNode, IOnDetachedFromNode
    {
        private Animator anim;
        //private bool Inited;
        public INode node;
        private ActionNodeSignalReceiver receiver;

        public AnimatorProcessorNodeData(INode node) { 
            this.node = node;
        }
        public void Attached(INode node)
        {
            if (node is NodeView view && view.TryGetComponent<Animator>(out anim)) {
                //Inited = true;
                Init();
            }
        }

        public void Detached(INode node)
        {
            receiver.SetActive(false);
        }

        private void Init() {
            receiver = node.RegisterSignalReceiver<IAnimatorProcessorSignal>((signal) => {
                
                signal.Process(anim);
            });
        }
    }

    public interface IAnimatorProcessorSignal : INodeSignal {
        public void Process(Animator anim);
    }

    public class SetAnimatorBoolValueSignal : IAnimatorProcessorSignal
    {
        public bool value;
        public string key;

        public void Process(Animator anim)
        {
            anim.SetBool(key,value);
        }
    }
}