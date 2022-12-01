using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	[RequireComponent(typeof(Camera))]
	public class SnapShot : MonoBehaviour
	{
		
		private Camera _camera;
		Camera myCamera{
			get{
				if(_camera == null)
					_camera = GetComponent<Camera>();
					
				return _camera;
			}
		}
		public static SnapShot shot;
		bool snapShotOnAwaken = false;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			_camera = GetComponent<Camera>();
			shot = this;
				
			if(snapShotOnAwaken) TakeSnapShot();
		}
		
		public static void TakeSnapShot(){
			if(shot != null){
				shot.myCamera.Render();
				if(shot.gameObject.activeSelf){
					shot.gameObject.SetActive(false);
				}
			}
			
			
		}
		
		public static void TakeSnapShot(Vector3 position,Quaternion rotation){
			
			if(shot != null){
				shot.myCamera.transform.position = position;
				shot.myCamera.transform.rotation = rotation;
			}
		}
		
		public static void SetPerspective(bool isOrthographic){
			if(shot != null){
				shot.myCamera.orthographic = isOrthographic;
			}
		}
	}
}