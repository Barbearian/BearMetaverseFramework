using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using UnityEngine.Events;
	public class SitNodeView : NodeView
	{
		static SitNodeView targetSit;
		public NodeView Anchor;
		public UnityEvent<bool> DOnSetFree;
		
		private SitNodeData sitData;
		bool free = true;
		public bool IsFree{
			get{
				return free;
			}
			
			private set{
				free = value;
				DOnSetFree.Invoke(free);
			}
		}
		int playerCount = 0;
		int sitterCount = 0;
		// Awake is called when the script instance is being loaded.
		public override void Awake()
		{
			base.Awake();
			if(Anchor == null){
				Anchor = new GameObject("SitAnchor").AddNodeView<NodeView>();
				this.AddNodeViewChild(Anchor);
			}
			
			var anchorData = Anchor.GetOrCreateNodeData<TransformAnchorNodeData>(new TransformAnchorNodeData());
			
			sitData = this.GetOrCreateNodeData<SitNodeData>(new SitNodeData());
			sitData.anchor = anchorData;
		}
		
		public void GoToSit(){
			targetSit = this;
			if(playerCount > 0){
				GlobalPlayerControllerSystem.EnterState(SitterNodeDataKeyword.GoToSeat);
				CheckThenSit();
				return;
			}
			
			GlobalPlayerControllerSystem.MoveTo(Anchor.transform.position);
			GlobalPlayerControllerSystem.EnterState(SitterNodeDataKeyword.GoToSeat);

		}
		
		public void CheckThenSit(){
			if(CanSit()){
				Sit();
			}
		}
		
		public void Sit(){
			GlobalPlayerControllerSystem.EnterState(SitterNodeDataKeyword.Sitting);
			GlobalPlayerControllerSystem.SnapTo(sitData.anchor);
		}
		
		public void OnTriggerEnter(Collider c){
			
			
			if(IsPlayer(c)){
				playerCount++;
				CheckThenSit();
			}
		}
		
		public void OnTriggerExit(Collider c){
			if(IsPlayer(c))
				playerCount--;
		}
		
		private bool CanSit(){
			if(
				GlobalPlayerControllerSystem.GetState().Equals(SitterNodeDataKeyword.GoToSeat)
				&&
				targetSit == this
			){
				return true;
			}
			
			return false;
		}
		
		public bool IsPlayer(Collider c){
			if(c.gameObject.TryGetComponent<NodeView>(out var view) && c.gameObject.CompareTag("Player")){
				return true;
			}else{
				return false;
			}
		}
		
		public void AddSitter(){
			sitterCount++;
			IsFree = false;

			if(sitterCount >1){
				GlobalPlayerControllerSystem.EnterState("Moving");
			}
		}
		
		public void RemoveSitter(){
			sitterCount--;
			Debug.Log("I removed sitter");
			if(sitterCount == 0){
				IsFree = true;
			}
		}
		
	}
	
	public static class SitNodeViewSystem{
		public static void SitOn(this NodeView view,NodeView Sit){
			if(Sit.TryGetNodeData<SitNodeData>(out var data)){
				view.SitOn(data);
			}
		}
		
	
	}
}