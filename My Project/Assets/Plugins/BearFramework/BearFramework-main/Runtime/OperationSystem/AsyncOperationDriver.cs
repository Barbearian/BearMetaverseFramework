using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	public class AsyncOperationDriver : MonoBehaviour
	{
		private static AsyncOperationDriver _driver;
		
		// Awake is called when the script instance is being loaded.
		protected void OnEnable()
		{
			if(_driver != null){
				Destroy(this);
				return;
			}
			
			_driver = this;
			AsyncOperationSystem.Initialize();
			DontDestroyOnLoad(this);
		}
		
		public void Update(){
			AsyncOperationSystem.Update();
		}
		
		public static void Schedule(AsyncOperationBase operation){
			Init();
			AsyncOperationSystem.StartOperation(operation);
			
		}
		
		public static void Init(){
			Debug.Log("I made driver");

			if(_driver == null){
				new GameObject("AsyncOperationDriver").AddComponent<AsyncOperationDriver>();
			}
		}
	}
}