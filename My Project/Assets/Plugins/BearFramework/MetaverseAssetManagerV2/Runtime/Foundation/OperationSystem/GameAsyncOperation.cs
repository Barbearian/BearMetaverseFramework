

namespace Bear{
	public abstract class GameAsyncOperation : AsyncOperationBase
	{
		internal override void Start()
		{
			OnStart();
		}
		internal override void Update()
		{
			OnUpdate();
		}
		
		/// <summary>
		/// Operation Start
		/// </summary>
		protected abstract void OnStart();

		/// <summary>
		/// Operation Update
		/// </summary>
		protected abstract void OnUpdate();
		
		/// <summary>
		/// Check whether system is busy
		/// </summary>
		protected bool IsBusy()
		{
			return OperationSystem.IsBusy;
		}
	}
}