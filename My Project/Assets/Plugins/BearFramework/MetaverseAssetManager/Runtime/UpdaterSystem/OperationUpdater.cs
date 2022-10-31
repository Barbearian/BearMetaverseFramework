using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class OperationUpdater : IUpdatable
    {

	    public bool Busy  => UpdatableSystem.Busy();
	    public readonly List<Operation> Processing = new List<Operation>();

        public void Clear()
        {
	        Processing.Clear();
        }

        public void Update()
        {
	        for (int i = 0; i < Processing.Count; i++) {
	        	var item = Processing[i];
	        	if(Busy) return;
	        	
	        	item.Update();
	        	if(!item.isDone) continue;
	        	
	        	Processing.RemoveAt(i);
	        	i--;
	        	if(item.status == OperationStatus.Failed) Logger.W("Unable to complete {0} with error: {1}", item.GetType().Name, item.error);
	        
	        	item.Complete();
	        }
        }
    }
}