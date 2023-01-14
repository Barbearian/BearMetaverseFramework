using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class BearAction
	{
		public ICost cost;
		public List<INodeSignal> DOnEnterSignals;
		
	}
	
	public interface ICost{
		public bool CanAfford();
		public void PayCost();
	}
}