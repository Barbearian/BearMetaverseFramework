using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.UIElements;
	using UnityEngine.Events;
	[RequireComponent(typeof(Collider))]
	public class MouseOverObjectHint : MonoBehaviour
	{
		
		public Transform anchor;
		public Vector3 offset;
		public UnityEvent DOnClick;
		bool showUI = true;
		bool overme;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			if(anchor == null){
				anchor = transform;
			}
		}
		
		public bool Shown = true;
		
		// OnMouseEnter is called when the mouse entered the GUIElement or Collider.
		protected void OnMouseEnter()
		{
			overme = true;
			if(showUI){
				HintUISystem.SetActive(true);
				HintUISystem.Register(()=>{DOnClick.Invoke();});
			}
		}
		
		// OnMouseOver is called every frame while the mouse is over the GUIElement or Collider.
		protected void OnMouseOver()
		{
			HintUISystem.MoveToWorldPosition(transform.position+offset);
		}
		
		// OnMouseEnter is called when the mouse entered the GUIElement or Collider.
		protected void OnMouseExit()
		{
			overme = false;

			HintUISystem.SetActive(false);

		}
		
		public void SetActiveHint(bool isActive){
			showUI = isActive;
			if(isActive && overme){
				HintUISystem.SetActive(true);
			}else{
				HintUISystem.SetActive(false);
			}
		}
		
		
		
		
	}
	

}