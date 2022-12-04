using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public interface INodeSignal
	{
	
	}
	
	public interface INodeSignalReceiver{
		public void Receive(INodeSignal signal);
		public bool IsActive{get;}
	}
	
	public class ActionNodeSignalReceiver:INodeSignalReceiver{
		public Action<INodeSignal> action;
		public void Receive(INodeSignal signal){
			try{
				action?.Invoke(signal);
			}catch(Exception e){
				Debug.LogWarning(e);
				isActive = false;
			}
		}
		
		public ActionNodeSignalReceiver(Action<INodeSignal> action){
			this.action = action;
			isActive = true;
		}
		
		bool isActive;
		public bool IsActive => isActive;
	}
}