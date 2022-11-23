using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.AI;
	using Cinemachine;
	public class MakeTestAvatar : MonoBehaviour
	{
		public NavMeshAgent agent;
		public CinemachineVirtualCamera cam;
		public Animator anim;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			var nanvView = NavMeshAgentControllerFactory.AddNavMeshAgentCharacterNodeData(Instantiate(agent));
			var animView = NavMeshAgentControllerFactory.AddAnimatorNodeData(Instantiate(anim),6,"Speed","MotionSpeed");
			var camView = CinemachineCameraFactory.AddCinemachineView(Instantiate(cam));
			if(animView.TryGetNodeData<AnimatorNodeData>(out var aData)){aData.clipData.EntryClip = new PlayAnimationClipInfo(){clipName = "Default"};}
			
			
			nanvView.AddGlobalPlayerControllerNodeData();
			nanvView.AddNavMeshAgentMovementInput();
			if(nanvView.TryGetNodeData<MovementNodeData>(out var data))data.speedMulti = 6;
			
			nanvView.LinkNavMeshAgentToAnimator(animView);
			nanvView.LinkInputToAnimator(animView);
			camView.Link(animView);
			
			SitterNodeDataSystem.AddSitterNodeData(nanvView,animView);
		}
	}
}