using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear{
    public interface IAnimatorClipsPlayer
    {
        void Play(int index);
    }
    
	public class AnimatorClipsPlayerNodeSignal:INodeSignal{
		public PlayAnimationClipInfo info;
	}
}