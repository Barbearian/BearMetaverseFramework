using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear {
    public class AnimatorNodeData : SignalHandlerNodeData, IOnAttachedToNode,IAnimatorClipsPlayer, IAnimatorNode
    {
        public Animator anim;
        public SafeDelegate<PlayAnimationClipInfo> dOnPlayedAnimation = new SafeDelegate<PlayAnimationClipInfo>();
        public SafeDelegate<int> dOnPlayedIndexed = new SafeDelegate<int>();
        public SafeDelegate dOnEnterDefaule = new SafeDelegate();
        public AnimationClipNodeData clipData;
        public Animator Anim { get => anim; set => anim = value; }
        public SafeDelegate<PlayAnimationClipInfo> DOnPlayedAnimation { get => dOnPlayedAnimation; set => dOnPlayedAnimation = value; }
        public SafeDelegate<int> DOnPlayedIndexed { get => dOnPlayedIndexed; set => dOnPlayedIndexed = value; }
        public SafeDelegate DOnEnterDefaule { get => dOnEnterDefaule; set => dOnEnterDefaule = value; }
        public AnimationClipNodeData ClipData { get => clipData; set => clipData = value; }

        public void Attached(INode node)
        {
            if (node is NodeView view) { 
	            anim = view.GetComponent<Animator>();
                
	            clipData = view.GetOrCreateNodeData(new AnimationClipNodeData());

                node.RegisterSignalReceiver<PlayAnimationNodeSignal>((x) => {
                    this.PlayInfo(x.info);
                },true).AddTo(receivers);

                node.RegisterSignalReceiver<PlayIndexedAnimationNodeSignal>((x) => {
                    this.Play(x.index);
                }, true).AddTo(receivers);
            }
        }

        public override void Detached(INode node)
        {
            base.Detached(node);
            anim = null;
        }

        public void Play(int index)
        {
            AnimatorNodeSystem.Play(this,index);
        }


    }
    public struct PlayAnimationNodeSignal : IAnimationSignal
    {
        public PlayAnimationClipInfo info;
    }

    public struct PlayIndexedAnimationNodeSignal : IAnimationSignal
    {
        public int index;
    }

    public interface IAnimationSignal:INodeSignal { }
    
}