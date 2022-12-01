
namespace Bear{
	using UnityEngine;
	public class SnapShotController : MonoBehaviour
	{
		
		public bool isOrthographic;
		public void TakeSnapShot(){
			SnapShot.SetPerspective(isOrthographic);
			SnapShot.TakeSnapShot(transform.position,transform.rotation);
		}
	}
}