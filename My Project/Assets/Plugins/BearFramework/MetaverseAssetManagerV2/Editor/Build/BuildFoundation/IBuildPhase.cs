using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset.Editor{
	public interface IBuildPhase{
		public void Start(BuildProduct product);
	}
}