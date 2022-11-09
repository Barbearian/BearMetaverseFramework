using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class BearJobProcessor
	{
		private Queue<BearJob> Waiting = new Queue<BearJob>();
		private List<BearJob> Progressing = new List<BearJob>();
	
		public bool busy;
		
		public void Schedule(BearJob job){
			Waiting.Enqueue(job);
		}
		
		public void Processing(){
			if(busy) return;
			
			while(Waiting.Count >0){
				var job = Waiting.Dequeue();
				if(job.status == BearJob.JobStatus.Paused) return;
				Progressing.Add(job);
				if(job.status == BearJob.JobStatus.Initializing) job.Start();
				
				if(busy) return;
			}
			
			for (int i = 0; i < Progressing.Count; i++) {
				var job = Progressing[i];
				if(job.isDone || job.status == BearJob.JobStatus.Paused){
					Progressing.RemoveAt(i);
					i--;
					
					if(job.isDone)
						job.Complete();
				}else{
					job.Progress();
				}
				
				if (busy) return;

			}
		}
		
		
	}
}