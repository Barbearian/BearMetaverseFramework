using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using Cinemachine;
	public class MakeCCAvatar : MonoBehaviour
	{
		public CharacterController cc;
		public CinemachineVirtualCamera cam;
		public Animator anim;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			var ccInstance = Instantiate(cc);
			ccInstance.transform.position = transform.position;
			
			var cView = ccInstance.AddCharacterControllerNodeData();
			var animView = NavMeshAgentControllerFactory.AddAnimatorNodeData(Instantiate(anim),6,"Speed","MotionSpeed");
			var camView = CinemachineCameraFactory.AddCinemachineView(Instantiate(cam),CinemachineBrain.UpdateMethod.FixedUpdate);
			if(animView.TryGetNodeData<AnimatorNodeData>(out var aData)){aData.clipData.EntryClip = new PlayAnimationClipInfo(){clipName = "Default"};}

			cView.AddCharacterControllerInput();
			if(cView.TryGetNodeData<MovementNodeData>(out var data))data.speedMulti = 3;

			cView.LinkCharacterControllerToAnimator(animView);
			cView.LinkInputToAnimator(animView);
			camView.Link(animView);

			cView.AddNodeData<SpeedUpNodeData>(new SpeedUpNodeData());
			cView.AddJumpAndRoll();
		}
	}
}