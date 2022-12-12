using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
	[RequireComponent(typeof(NodeView))]
    public class AnimatorClipsDataInjector : MonoBehaviour
	{
		public AnimatorClipsData data;
	    // Awake is called when the script instance is being loaded.
	    protected void Awake()
	    {
	    	if(gameObject.TryGetComponent<NodeView>(out var view)){
	    		var nodeData = view.GetOrCreateNodeData(new AnimationClipNodeData());
	    		nodeData.EntryClip = data.clips.EntryClip;
	    		nodeData.defaultClips = data.clips.defaultClips;
	    	}
	    }
    }
}
