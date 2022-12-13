using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear{
	using UnityEngine.InputSystem;
	[RequireComponent(typeof(Animator))]
	public class AnimatorInputController: MonoBehaviour
	{
		private Animator anim;
		public string AnimatorAttribute;
		public ClampAndMultiplier Config;
		public InputActionReference CameraZoom;

		public float current;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			anim = GetComponent<Animator>();
			CameraZoom.action.Enable();
		}

		
		private void UpdateValue(InputAction.CallbackContext context){
			float delta = context.ReadValue<float>();
			current += Config.Multiplier*delta*Time.deltaTime;
			current = Mathf.Clamp(current,Config.MinValue,Config.MaxValue);
			
			anim.SetFloat(AnimatorAttribute,current);
		}
		
		private void OnEnable() {
			anim.SetFloat(AnimatorAttribute,current);
			CameraZoom.action.performed += UpdateValue;
		}

		private void OnDisable() {
			CameraZoom.action.performed -= UpdateValue;
		}
		
	}
}