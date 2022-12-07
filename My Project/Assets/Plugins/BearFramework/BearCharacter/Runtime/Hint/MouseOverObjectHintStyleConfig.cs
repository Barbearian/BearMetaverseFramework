using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	[CreateAssetMenu(menuName = "HintStyle",fileName = "HintStyle")]
	public class MouseOverObjectHintStyleConfig : ScriptableObject
	{
		public Sprite mySprite;
		public Vector3 myOffset;

		public virtual Sprite GetSprite(){
			return mySprite;
		}
		
		public virtual Vector3 GetOffset(){
			return myOffset;
		}

	}
}