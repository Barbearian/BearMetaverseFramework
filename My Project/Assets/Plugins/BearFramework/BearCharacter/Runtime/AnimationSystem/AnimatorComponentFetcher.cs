using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
	public class AnimatorComponentFetcher 
	{
		bool createWhenNonExist;
		bool fetched;
		Component rs;
		
		public AnimatorComponentFetcher(bool createWhenNonExist = false){
			this.createWhenNonExist = createWhenNonExist;
		}
		
		public T GetComponent<T>(GameObject obj,bool forcedFetch = false)
	    where T:Component
		{
		
			if(fetched){
				return rs as T;	
			}
	    	
	    	if((!fetched) || forcedFetch){
	    		rs = obj.GetComponent<T>();
	    		fetched = true;
	    	}
	    	
			if(!fetched && createWhenNonExist){
				rs = obj.AddComponent<T>();
				fetched = true;
			}
	    	
	    	return rs as T;
	    }
    }
}
