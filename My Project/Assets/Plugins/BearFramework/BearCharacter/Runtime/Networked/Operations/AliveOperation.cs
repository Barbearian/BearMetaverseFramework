using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AliveOperation : AsyncOperationBase
	{
		float time;
		float left;
		public AliveOperation(float time){
			this.time = time;

		}
		
		public void Reset(){
			left = time;
		}
		
		public void Kill(){
			Status = EOperationStatus.Failed;
			
		}
		
		public override void Update()
		{
			left -= Time.deltaTime;
			if(left<=0){
				left = 0;
				Status = EOperationStatus.Succeed;
			}
		}
		
		public override void Start()
		{
			Reset();
		}
	}
}