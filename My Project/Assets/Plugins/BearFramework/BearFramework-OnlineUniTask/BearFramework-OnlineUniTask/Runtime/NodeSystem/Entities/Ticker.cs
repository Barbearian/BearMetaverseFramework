using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class Ticker
	{
		float remain;
		float max;
		Action action;
		public Ticker(Action action,float time = 1){
			this.action = action;
			remain = time;
			max = time;
		}
		
		public void Tick(){
			remain -= Time.deltaTime;
			
			if(remain<=0){
				action.Invoke();
				remain = max;
			}
		}
	}
}