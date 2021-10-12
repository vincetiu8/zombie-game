using Photon.Pun;
using UnityEngine;

namespace Interact
{
	public abstract class HoldInteractable : IconInteractable
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

			Debug.Log("Starting Interaction!");

			photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, true);
			LocallyInteracting = true;
		}

		public override void CancelInteraction()
		{
			if (!LocallyInteracting) return;

			photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, false);
			LocallyInteracting = false;
		}

		/// <summary>
		///     Allows the player to move normally again, exception used to make sure this can only get called once to avoid repeat
		///     problems
		/// </summary>
		protected virtual void FinishInteraction()
		{
			// Notifies the player the interaction has finished
			finishInteraction.Invoke();

			photonView.RPC("RPCSetAvailableForInteract", RpcTarget.All, false);
			LocallyInteracting = false;
		}

		[PunRPC]
		protected void RPCSetAvailableForInteract(bool availableForInteract)
		{
			_availableForInteract = availableForInteract;
		}
	}
}