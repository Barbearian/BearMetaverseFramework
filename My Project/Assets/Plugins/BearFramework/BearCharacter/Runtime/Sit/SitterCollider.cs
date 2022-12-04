using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	[RequireComponent(typeof(Collider),typeof(Rigidbody))]
	public class SitterCollider : MonoBehaviour
	{
		
		SitNodeView tview;
		// OnTriggerEnter is called when the Collider other enters the trigger.
		protected void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent<SitNodeView>(out var view)){
				view.AddSitter();
				tview = view;
			}
		}
		
		protected void OnTriggerExit(Collider other){
			if(other.TryGetComponent<SitNodeView>(out var view)){
				if(tview == view){
					tview.RemoveSitter();
					tview = null;
				}
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			if(tview != null){
				tview.RemoveSitter();
			}
			tview = null;

		}
	
	}
	
	public static class SitterColliderSystem{
		public static SitterCollider GetSitterCollider(this GameObject obj,SitterCollider defaultCollider ){
			var rs = obj.GetComponentInChildren<SitterCollider>();
			if(rs == null){
				rs = MonoBehaviour.Instantiate(defaultCollider,obj.transform);
			}
			
			return rs;
		}
	}
}