using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class LoadableUpdater : IUpdatable
	{
		public Func<bool> IsBusy {get;set;} = UpdatableSystem.Busy;
		public readonly List<Loadable> Loading = new List<Loadable>();
		public readonly List<Loadable> Unused = new List<Loadable>();	
		public void Update(){
			for (var index = 0; index < Loading.Count; index++)
			{
				var item = Loading[index];
				if (IsBusy()) return;

				item.Update();
				if (!item.isDone) continue;

				Loading.RemoveAt(index);
				index--;
				item.Complete();
			}
			
			for (int index = 0, max = Unused.Count; index < max; index++)
			{
				var item = Unused[index];
				if (IsBusy()) break;

				if (!item.isDone) continue;

				Unused.RemoveAt(index);
				index--;
				max--;
				if (!item._reference.unused) continue;

				item.Unload();
			}
		}

        public void Clear()
        {
			Loading.Clear();
			Unused.Clear();
        }
    }
}