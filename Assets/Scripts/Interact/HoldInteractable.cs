using Photon.Pun;

namespace Interact
{
	public abstract class HoldInteractable : Interactable
	{
		private   bool _availableForInteract;
		protected bool LocallyInteracting;

		protected virtual void Awake()
		{
			_availableForInteract = true;
		}

		public override void StartInteraction()
		{
			if (!_availableForInteract) return;

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
			// Notifies the player the interaction has finished
			finishInteraction.Invoke();

			ToggleInteraction(false);
		}

		private void ToggleInteraction(bool toggle)
		{
			photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, !toggle);
			LocallyInteracting = toggle;
		}

		[PunRPC]
		protected void RPCSetAvailableForInteract(bool availableForInteract)
		{
			_availableForInteract = availableForInteract;
		}
	}
}