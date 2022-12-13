using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using Cinemachine;

	[RequireComponent(typeof(CinemachineVirtualCamera))]

	public class CinemachineController : MonoBehaviour
	{        
	
	    	    	
		private CinemachineVirtualCamera _cam;
		bool inited;
		protected CinemachineVirtualCamera cam{
			get{
				if(!inited){
					_cam = GetComponent<CinemachineVirtualCamera>();
					inited = true;
				}
				return _cam;
			}
		}
		public ClampAndMultiplier FOVConfig;


	
		public void UpdateFovByPercentage(float value){
			value = Mathf.Abs(value);
			cam.m_Lens.FieldOfView = FOVConfig.GetValueByPercentage(value);
		}
		
		public void UpdateFovByPercentageInverted(float value){
			cam.m_Lens.FieldOfView = FOVConfig.GetValueByPercentage(1-Mathf.Abs(value));

		}
		 

	}
}