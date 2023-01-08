using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class TriggerNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode
	{
		protected NodeView view;
		private TriggerObserver observer;
		public virtual void OnTriggerEnter(Collider collider){
			
		}
		
		public virtual void OnTriggerExit(Collider collider){
			
		}
		
		public void Attached(INode node){
			if(node is NodeView view && view.gameObject.TryGetComponent<Collider>(out var collier)){
				this.view = view;
				if(!view.TryGetComponent<TriggerObserver>(out var observer)){
					observer = view.gameObject.AddComponent<TriggerObserver>();
				}
				
				this.observer = observer;
				observer.DOTriggerEnter += OnTriggerEnter;
				observer.DOTriggerExit += OnTriggerExit;
			}
		}
		
		public void Detached(INode node){
			if(observer != null){
				observer.DOTriggerEnter -= OnTriggerEnter;
				observer.DOTriggerExit -= OnTriggerExit;
			}
		}
	}
}