
using UnityEngine;
namespace Bear
{
    public class AnimatorLinkNodeData : LinkNodeData
    {
        public override (string, string) GetKeys()
        {
            return GetKeys<AnimatorLinkSignal, AnimatorDelinkSignal>();
        }

        public override void Attached(INode node)
        {
            base.Attached(node);
            node.RegisterSignalReceiver<PlayAnimationNodeSignal>((x) => {
                Target.ReceiveNodeSignal(x);
            }, true).AddTo(receivers);
        }
    }

    public struct AnimatorLinkSignal : ILinkSignal
    {
        public INode Target { get; set; }
    }

    public struct AnimatorDelinkSignal : IDelinkSignal
    {
        public INode Target { get; set; }
    }
}