using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Bear{
    public interface ICinemachineBrainAccessor{

    }

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineVirtualCameraNodeView : NodeView,ICinemachineBrainAccessor
    {
        public CinemachineVirtualCamera cam;
        public CameraTargetNodeData dcland;

        
	    public override void Awake() {
	    	base.Awake();
            cam = GetComponent<CinemachineVirtualCamera>();

            cam.LookAt = dcland.lookAt;
            cam.Follow = dcland.follow;
            
            dcland.DSetLookAt += this.UpdateLookAt;
            dcland.DSetFollow += this.UpdateFollow;            
        }
        
    }

    public static class CinemachineVirtualCameraNodeViewSystem{

        public static void UpdateLookAt(this CinemachineVirtualCameraNodeView view, Transform transform){
            view.cam.LookAt = transform;
        }

        public static void UpdateFollow(this CinemachineVirtualCameraNodeView view, Transform transform){
            view.cam.Follow = transform;
        }
    }


	public struct CinemachineBrainNodeData:INodeData,IOnAttachedToNode,IOnDetachedFromNode{
	    public CinemachineBrain brain;
        
		public void Attached(INode node){
			var cam = Camera.main;
			if(!cam.TryGetComponent<CinemachineBrain>(out var brain)){
				cam.gameObject.AddComponent<CinemachineBrain>();
			}
		}
		
		public void Detached(INode node){
			Debug.Log("Cinemachine Camera detached");
		}
    }

    public static class CinemachineBrainNodeDataSystem{
        public static void Init(this CinemachineBrainNodeData data){
            if(data.brain != null){
                return;
            }

            Camera cam = Camera.main;

            if(!cam.TryGetComponent<CinemachineBrain>(out var brain)){
                data.brain = cam.gameObject.AddComponent<CinemachineBrain>();
            }else{
                data.brain = brain;
            }
        }
    }
}