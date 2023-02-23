using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.AI;
	using Cinemachine;
	public class MakeNetworkedAvatar : MonoBehaviour
	{


		public NavMeshAgent agent;
		public CinemachineVirtualCamera cam;
		public Animator anim;
		public AnimationClipNodeData adata;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			MakeClient();
			MakeNetworkedPlayer();
		}
		
		public void MakeClient(){
			var nanvView = NavMeshAgentControllerFactory.AddNavMeshAgentCharacterNodeData(Instantiate(agent));
			var animView = NavMeshAgentControllerFactory.AddAnimatorNodeData(Instantiate(anim),6,"Speed","MotionSpeed");
			if(animView.TryGetNodeData<AnimatorNodeData>(out var aData)){
				aData.clipData.EntryClip = adata.EntryClip;
				aData.clipData.defaultClips = adata.defaultClips;
			}


			var camView = CinemachineCameraFactory.AddCinemachineView(Instantiate(cam));
		
		
			nanvView.AddGlobalPlayerControllerNodeData();
			nanvView.AddNavMeshAgentMovementInput();
			if(nanvView.TryGetNodeData<MovementNodeData>(out var data))data.speedMulti = 6;
		
			nanvView.LinkNavMeshAgentToAnimator(animView);
			nanvView.LinkInputToAnimator();
			camView.Link(animView);

			SitterNodeDataSystem.AddSitterNodeData(nanvView,animView,1);
			
			nanvView.AddNodeData<NetworkedObjectNodeData>(new NetworkedObjectNodeData("Bear"));
			nanvView.AddNodeData<NetworkedNavigatorNodeData>(new NetworkedNavigatorNodeData(animView));
				
		}
		
		public void MakeNetworkedPlayer(){
			var nanvView = NavMeshAgentControllerFactory.AddNavMeshAgentCharacterNodeData(Instantiate(agent));
			var animView = NavMeshAgentControllerFactory.AddAnimatorNodeData(Instantiate(anim),6,"Speed","MotionSpeed");
			if(animView.TryGetNodeData<AnimatorNodeData>(out var aData)){
				aData.clipData.EntryClip = adata.EntryClip;
				aData.clipData.defaultClips = adata.defaultClips;
			}

			//if(animView.TryGetNodeData<AnimatorNodeData>(out var aData)){aData.clipData.EntryClip = new PlayAnimationClipInfo(){clipName = "Default"};}
		
		
			if(nanvView.TryGetNodeData<MovementNodeData>(out var data))data.speedMulti = 6;
		
			nanvView.LinkNavMeshAgentToAnimator(animView);
			nanvView.LinkInputToAnimator();
			
			nanvView.AddNodeData<NetworkedObjectNodeData>(new NetworkedObjectNodeData("Bear",NetworkedObjectType.networked));
			nanvView.AddNodeData<NetworkedNavigatorNodeData>(new NetworkedNavigatorNodeData(animView));



			nanvView.tag = "Untagged";
		
		}
	}
	
}