using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class SitterAnimatorBehavior : StateMachineBehaviour
	{
		public SitterCollider pref;
		SitterCollider _col;
		bool DisableOnEnd = true;
		private SitterCollider GetCol(GameObject obj){
			if(_col == null){
				_col = obj.GetSitterCollider(pref);
			}
			return _col;
		}
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			GetCol(animator.gameObject).gameObject.SetActive(true);
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(DisableOnEnd)
				GetCol(animator.gameObject).gameObject.SetActive(false);

		}
	}
}