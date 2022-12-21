using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class AlivePoolNodeView : NodeView
	{
		private Dictionary<string,AliveOperation> operations = new Dictionary<string,AliveOperation>();
		// This function is called when the MonoBehaviour will be destroyed.
		protected void OnDestroy()
		{
			foreach (var item in operations.Values)
			{
				item.Kill();
			}	
		}
		
		public void Ping(string id, Action DOnEnd,float life = 10f){
			if(operations.TryGetValue(id,out var op)){
				op.Reset();
			}else{
				op = new AliveOperation(life);
				op.Completed +=(x)=>{
					DOnEnd.Invoke();
					operations.Remove(id);
				};
				operations[id] = op;
				AsyncOperationDriver.Schedule(op);
				
			}
		}
		
		
	}
}