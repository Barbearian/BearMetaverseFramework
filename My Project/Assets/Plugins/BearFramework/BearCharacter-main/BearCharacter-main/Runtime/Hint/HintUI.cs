using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.EventSystems;
	using UnityEngine.InputSystem;
	using UnityEngine.UI;
	[RequireComponent(typeof(RectTransform))]
	public class HintUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
	{
		public Button button;
		public InputActionReference reference;
		public static Camera Cam{get{
			if(_cam==null)
				_cam = Camera.main;	
			return _cam;
		}}
		static Camera _cam;
		
		private RectTransform rectTrans;
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			Physics.queriesHitTriggers = true;

			rectTrans = GetComponent<RectTransform>();
			
			if(HintUISystem.Hint != null){
				Destroy(HintUISystem.Hint);
			}
			HintUISystem.Hint = this;
			
			this.gameObject.SetActive(false);
		}
		
		public void MoveTo(Vector3 position){
			//Debug.Log("LOl");
			rectTrans.position = Cam.WorldToScreenPoint(position);
		}
		
		public void OnPointerEnter(PointerEventData pointerEventData)
		{
			InputHelper.GetAction(reference.action.name).Disable();
		}

		//Detect when Cursor leaves the GameObject
		public void OnPointerExit(PointerEventData pointerEventData)
		{
			InputHelper.GetAction(reference.action.name).Enable();

		}
		
		public void OnDisable(){
			InputHelper.GetAction(reference.action.name).Enable();

		}
		
		public void Register(System.Action action){
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(()=>{action.Invoke();});
		}
	}
	
	public static class HintUISystem{
		
		public static HintUI Hint;
		

		public static void MoveToWorldPosition(Vector3 position){
			//Debug.Log("Moved");
			if(Hint!=null) 
				Hint.MoveTo(position);
		}
		
		public static void SetActive(bool isActive){
			if(Hint!=null) 
				Hint.gameObject.SetActive(isActive);
		}
		
		public static void Register(System.Action action){
			if(Hint!=null) 
				Hint.Register(action);
		}
	}
}