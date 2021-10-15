using Photon.Pun;
using UnityEngine;

namespace Interact
{
	public abstract class HoldInteractable : Interactable
	{
		private   bool _availableForInteract;
		protected bool LocallyInteracting;

		private void Awake()
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
			Debug.Log(LocallyInteracting);
			if (!LocallyInteracting) return;

			FinishInteraction();
		}

		/// <summary>
		///     Allows the player to move normally again, exception used to make sure this can only get called once to avoid repeat
		///     problems
		/// </summary>
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