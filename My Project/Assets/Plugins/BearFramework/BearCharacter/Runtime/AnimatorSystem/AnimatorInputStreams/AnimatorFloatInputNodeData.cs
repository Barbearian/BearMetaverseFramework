using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class AnimatorFloatInputNodeData : INodeData,IOnAttachedToNode,IOnDetachedFromNode,IAnimatorFloatInputReceiver
	{
		public string FloatAttribute;
		public SafeDelegate<float>  DOnUpdateValue= new SafeDelegate<float>();
		public Dictionary<IAnimatorNode,Action> links = new Dictionary<IAnimatorNode, Action>();

		public void Attached(INode node){
			if (node is IAnimatorNode nodeview)
			{
				this.Link(nodeview);
			}
			else {
				var animND = node.GetOrCreateNodeData(new AnimatorNodeData());
				this.Link(animND);
			}
		}
		
		public void Detached(INode node){
			
		}
		
		public void Update(float value){
			DOnUpdateValue.invoker.Invoke(value);
		}
	}
	
	public interface IAnimatorFloatInputReceiver{
		void Update(float value);
	}
	
	public static class IAnimatorFloatInputReceiverSystem{
		
		public static void UpdateFloatParameter(this IAnimatorFloatInputReceiver receiver,float value){

			receiver.Update(value);
		}
		
		public static void Link(this AnimatorFloatInputNodeData nodedata, IAnimatorNode view){
                

			var Delink = nodedata.DOnUpdateValue.Link<float>((x)=>{
				view.SetFloat(nodedata.FloatAttribute,x);
			});
                 

			nodedata.Release(view);
			nodedata.links[view] = Delink;
		}
		
		public static void Release(this AnimatorFloatInputNodeData nodedata, IAnimatorNode view){
			if(nodedata.links.TryGetValue(view,out var Delink)){
				try{
					Delink.Invoke();
				}catch(Exception e){
					Debug.LogWarning(e);
				}

				nodedata.links.Remove(view);
			}
		}
	}
}