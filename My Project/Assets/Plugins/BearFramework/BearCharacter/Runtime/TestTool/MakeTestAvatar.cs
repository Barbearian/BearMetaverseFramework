using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.AI;
	using Cinemachine;
	public class MakeTestAvatar : MonoBehaviour
	{
		public NavMeshAgent agent;
		public CinemachineVirtualCameraBase cam;
		public Animator anim;
		public AnimationClipNodeData adata;

		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			var agentInstance = Instantiate(agent);
			agentInstance.TeleportTo(transform.position);
			
			
			var nanvView = NavMeshAgentControllerFactory.AddNavMeshAgentCharacterNodeData(agentInstance);
			var animView = NavMeshAgentControllerFactory.AddAnimatorNodeData(Instantiate(anim),6,"Speed","MotionSpeed");
			var camView = CinemachineCameraFactory.AddCinemachineView(Instantiate(cam));
			if(animView.TryGetNodeData<AnimatorNodeData>(out var aData)){
				aData.clipData.EntryClip = adata.EntryClip;
				aData.clipData.defaultClips = adata.defaultClips;
			}

			
			nanvView.AddGlobalPlayerControllerNodeData();
			nanvView.AddNavMeshAgentMovementInput();
			if(nanvView.TryGetNodeData<MovementNodeData>(out var data))data.speedMulti = 3;
			
			nanvView.LinkNavMeshAgentToAnimator(animView);
			nanvView.LinkInputToAnimator();
			camView.Link(animView);
			
			SitterNodeDataSystem.AddSitterNodeData(nanvView,animView);
			
			nanvView.AddNodeData<SpeedUpNodeData>(new SpeedUpNodeData(){
				DOnSpeedUp = (x)=>{
					agentInstance.speed = x;
				}
			});
		}
		
	
	}
	
	public static class NavAgentSystem{
		public static void TeleportTo(this NavMeshAgent agent,Vector3 position){
			agent.gameObject.SetActive(false);
			agent.transform.position = position;
			agent.gameObject.SetActive(true);

		}
	}
}