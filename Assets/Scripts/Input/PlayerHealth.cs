using Interact;

namespace Input
{
	public class PlayerHealth : HealthController
	{
		private PlayerInteract _playerInteract;

		private void Start()
		{
			_playerInteract = transform.GetComponent<PlayerInteract>();
		}

		// Makes it so that taking damaged also cancels current input 
		public override void ChangeHealth(int change)
		{
			if (change < 0) _playerInteract.CancelInteraction();
			base.ChangeHealth(change);
		}
	}
}