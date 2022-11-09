using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public interface IRecyclable
	{
		void EndRecycle();
		bool CanRecycle();
		void RecycleAsync();
		bool Recycling();
	}
	
	public class Recycler : MonoBehaviour
	{
		private static readonly Dictionary<int, IRecyclable> Recyclables = new Dictionary<int, IRecyclable>();
		private static readonly List<IRecyclable> Progressing = new List<IRecyclable>();
		private static readonly Queue<IRecyclable> Unused = new Queue<IRecyclable>();
		
		
		private void Update(){
			if(Scheduler.Working) return;
			
			foreach (var item in Recyclables)
			{
				var request = item.Value;
				if(!request.CanRecycle()) continue;
				
				request.RecycleAsync();
				Unused.Enqueue(request);
			}
			
			while(Unused.Count>0){
				var request = Unused.Dequeue();
				Recyclables.Remove(request.GetHashCode());
				Progressing.Add(request);
			}
			
			for (int i = 0; i <  Progressing.Count; i++) {
				var request = Progressing[i];
				if (request.Recycling()) continue;
				Progressing.RemoveAt(i);
				i--;
				if (request.CanRecycle()) request.EndRecycle();
				if (Scheduler.Busy) return;

			}
		}
		
		public static void RecycleAsync(IRecyclable recyclable)
		{
			Recyclables[recyclable.GetHashCode()] = recyclable;
		}

		public static void CancelRecycle(IRecyclable recyclable)
		{
			Recyclables.Remove(recyclable.GetHashCode());
		}
	}
}