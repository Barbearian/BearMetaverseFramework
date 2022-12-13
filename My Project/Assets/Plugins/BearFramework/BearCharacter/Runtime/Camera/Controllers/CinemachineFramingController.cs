using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using Cinemachine;
	public class CinemachineFramingController : CinemachineController
	{

		
		public ClampAndMultiplier DistanceConfig;
		
		bool cftInited;
		private CinemachineFramingTransposer _cft;

		private CinemachineFramingTransposer cft{
			get{
				if(!cftInited ){
					cftInited = true;
					_cft = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
					_cft.m_CameraDistance = DistanceConfig.GetValue(cft.m_CameraDistance );

				}
				
				return _cft;
			}
		}

		public void UpdateDistance(float distance){

			cft.m_CameraDistance = DistanceConfig.GetValue(distance);
		}
		
		public void UpdateDistanceByPercentage(float distance){
			distance = Mathf.Abs(distance);
			cft.m_CameraDistance = DistanceConfig.GetValueByPercentage(distance);
		}
	}
}