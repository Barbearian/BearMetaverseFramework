using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
	[CreateAssetMenu(menuName = "NodeData/AnimatorClipsData",fileName= "AnimatorClipsData")]
	public class AnimatorClipsData : ScriptableObject
    {
	    public AnimationClipNodeData clips;
    }
}
