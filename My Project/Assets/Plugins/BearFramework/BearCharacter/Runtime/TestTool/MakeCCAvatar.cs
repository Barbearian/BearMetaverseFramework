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
			cc.gameObject.SetActive(false);
			ccInstance.transform.position = transform.position;
            cc.gameObject.SetActive(true);

            var cView = ccInstance.AddCharacterControllerNodeData();
			var animView = NavMeshAgentControllerFactory.AddAnimatorNodeData(Instantiate(anim),6,"Speed","MotionSpeed");
			var camView = CinemachineCameraFactory.AddCinemachineView(Instantiate(cam),CinemachineBrain.UpdateMethod.FixedUpdate);
			if(animView.TryGetNodeData<AnimatorNodeData>(out var aData)){aData.clipData.EntryClip = new PlayAnimationClipInfo(){clipName = "Default"};}

			cView.AddCharacterControllerInput();
			if(cView.TryGetNodeData<MovementNodeData>(out var data))data.speedMulti = 3;

			cView.LinkCharacterControllerToAnimator(animView);
			cView.LinkInputToAnimator(animView);
			camView.Link(animView);

			cView.AddNodeData(new SpeedUpNodeData());
			cView.AddJumpAndRoll();

			//add pick
			cView.AddNodeData(new ItemPickerNodeData());

			//add equipmentmanager
			cView.AddNodeData(new ItemManagerNodeData());
            cView.AddNodeData(new ItemInputNodeData());
			cView.AddNodeData(new WielderToItemNodeData());

            //add finder
            cView.AddNodeData(new FinderNodeData());

			//add execution filter
			cView.AddNodeData(new ExecutionFilterNodeData());

			//add animator signal edge
			var AnimatorLinkNodeData = new AnimatorLinkNodeData();

			cView.AddNodeData(AnimatorLinkNodeData);
			AnimatorLinkNodeData.Link(animView);

			//add adjust facedir
			cView.AddNodeData(new CharacterDirectionAdjustNodeData());


        }
	}
}