using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
	using Cinemachine;
    public static class CinemachineCameraFactory 
    {
        //public static async UniTask<CinemachineVirtualCameraNodeView> MakeLocalLookatCamera()
        //{
        //    if (INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader))
        //    {
        //        //Make Camera 
        //        var cam = Camera.main;
        //        var camview = cam.gameObject.AddNodeView<CameraNodeView>();
        //        camview.Init();

        //        //Make Camera look at player
        //        var cnode = await loader.LoadAsync<GameObject>("CinemachineFreeForm");
        //        var camNode = GameObject.Instantiate(cnode).GetComponent<CinemachineVirtualCameraNodeView>();
        //        return camNode;
        //    }
        //    return null;
        //}
        
	    public static CinemachineVirtualCameraNodeView AddCinemachineView(Cinemachine.CinemachineVirtualCamera camera, CinemachineBrain.UpdateMethod method = CinemachineBrain.UpdateMethod.SmartUpdate){
		    //Make Camera 
		    var cam = Camera.main;
		    var camview = cam.gameObject.AddNodeView<CameraNodeView>();
		    camview.AddNodeData(new CinemachineBrainNodeData(){
		    	updateMethod = method
		    });
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