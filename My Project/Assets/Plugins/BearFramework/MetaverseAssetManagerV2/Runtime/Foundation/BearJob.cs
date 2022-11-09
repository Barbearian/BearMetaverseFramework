using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class BearJob
	{
		public enum JobStatus{
			Initializing,
			Progressing,
			Paused,
			Completed,

		}
	
		public enum JobResult{
			Default,
			Success,
			Failed,
			Cancelled
		}
		
		public JobResult result {get;protected set;} = JobResult.Default;
		public JobStatus status {get;protected set;} = JobStatus.Initializing;
		public float progress = 0;
		public string error;
		public Action<BearJob> completed;

		public bool isDone => status == JobStatus.Completed;

		public void Start(){
			if (status != JobStatus.Initializing) return;
			status = JobStatus.Progressing;
			OnStart();
		}
		
		public void Progress(){
			OnProgress();
		}
	
		public void Pause(){
			OnPause();
		}
	
		public void Cancel(){
			OnCancel();
		}
		

		public void Complete(){
			OnComplete();
			var saved = completed;
			completed?.Invoke(this);
			completed -= saved;
		}
		
		public void SetResult(JobResult result,string err = null){
			this.result = result;
			this.status = JobStatus.Completed;
			progress = 1;
			this.error = err;
		}

		
		protected virtual void OnStart(){
			
		}
		
		protected virtual void OnCancel(){
			
		}
		
		protected virtual void OnProgress(){
			
		}
		
		protected virtual void OnPause(){
			
		}
		
		protected virtual void OnComplete(){
			
		}
		
		public virtual void Clear(){}
		
		
		
		
		
	}
	

}