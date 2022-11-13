using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.AI;
	using Cysharp.Threading.Tasks;
	public class BearControllerExample : MonoBehaviour
	{
		public NavMeshAgent agent;
		public Animator playerPref;
		public Cinemachine.CinemachineVirtualCamera cameraPref;
		public async void Start(){
			var view = NavMeshAgentControllerFactory.AddNavMeshAgentCharacterNodeData(Instantiate(agent));
			view.AddNavMeshAgentMovementInput();
			if(view.TryGetNodeData<MovementNodeData>(out var data)){data.speedMulti = 4;}
		
			
			var anim = Instantiate(playerPref);
			var animView = NavMeshAgentControllerFactory.AddAnimatorNodeData(anim,6,"Speed","MotionSpeed");
			view.LinkNavMeshAgentToAnimator(animView);

			var camera = CinemachineCameraFactory.AddCinemachineView(Instantiate(cameraPref));
			CinemachineCameraFactory.Link(camera,animView);
			
			
		}
	}
}