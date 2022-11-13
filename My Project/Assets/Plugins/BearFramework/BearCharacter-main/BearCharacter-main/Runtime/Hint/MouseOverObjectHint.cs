using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.UIElements;
	[RequireComponent(typeof(Collider))]
	public class MouseOverObjectHint : MonoBehaviour
	{
		
		public Transform anchor;
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
			HintUISystem.SetActive(true);
		}
		
		// OnMouseOver is called every frame while the mouse is over the GUIElement or Collider.
		protected void OnMouseOver()
		{
			HintUISystem.MoveToWorldPosition(transform.position);
		}
		
		// OnMouseEnter is called when the mouse entered the GUIElement or Collider.
		protected void OnMouseExit()
		{
			HintUISystem.SetActive(false);

		}
		
		
	}
	

}