using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public static class CinemachineCameraFactory 
    {

        
	    public static CinemachineVirtualCameraNodeView AddCinemachineView(Cinemachine.CinemachineVirtualCamera camera){
		    //Make Camera 
		    var cam = Camera.main;
		    var camview = cam.gameObject.AddNodeView<CameraNodeView>();
		    camview.AddNodeData(new CinemachineBrainNodeData());
		    camview.Init();
		    
		    var camNode = camera.gameObject.AddNodeView<CinemachineVirtualCameraNodeView>();
		    return camNode;

	    }
        
        public static void Link(this CinemachineVirtualCameraNodeView view, NodeView target)
        {
            if (!target.TryGetKidNode<CameraAnchorNodeView>(out var anchor))
            {
                anchor = new GameObject("CameraAnchor").AddNodeView<CameraAnchorNodeView>();
                target.AddNodeViewChild(anchor);
                anchor.transform.localPosition = Vector3.up * 1.5f;
            }

            

            view.dcland.UpdateFollow(anchor.transform);
            view.dcland.UpdateLookAt(anchor.transform);
        }
    }
}