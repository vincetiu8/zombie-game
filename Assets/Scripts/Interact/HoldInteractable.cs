using Photon.Pun;

namespace Interact
{
	public abstract class HoldInteractable : Interactable
	{
		protected bool AvailableForInteract;
		protected bool LocallyInteracting;

		protected override void Start()
		{
			base.Start();
			RPCSetAvailableForInteract(true);
		}

		public override void StartInteraction()
		{
			if (!AvailableForInteract) return;

			// Notifies the player the interaction has started
			startInteraction.Invoke();

			ToggleInteraction(true);
		}

		public override void CancelInteraction()
		{
			if (!LocallyInteracting) return;

			FinishInteraction();
		}

		protected virtual void FinishInteraction()
		{
			ToggleInteraction(false);

			// Notifies the player the interaction has finished
			finishInteraction.Invoke();
		}

		private void ToggleInteraction(bool toggle)
		{
			photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, !toggle);
			LocallyInteracting = toggle;
		}

		[PunRPC]
		protected virtual void RPCSetAvailableForInteract(bool available)
		{
			AvailableForInteract = available;
		}

		public bool AbleToInteract()
		{
			return AvailableForInteract || LocallyInteracting;
		}
	}
}