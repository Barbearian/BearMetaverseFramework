using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class MaskLayerOnEnter : StateMachineBehaviour
	{
		public LayerMask mask;
		private LayerMask old;
	
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var cam = GLobalCameraCam.GetMainCam();
			old = cam.cullingMask & mask;
			cam.cullingMask = (cam.cullingMask | mask) - mask;
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var cam = GLobalCameraCam.GetMainCam();
			cam.cullingMask = (cam.cullingMask | mask)-mask + old;
		}
	}
	
	public static class GLobalCameraCam{
		private static Camera cam;
		
		public static Camera GetMainCam(){
			if(cam == null){
				cam = Camera.main;
			}
			return cam;

		}
	}
}