using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public class BuildSchedule
	{
		public List<IBuildPhase> phases = new List<IBuildPhase>();
	}
}