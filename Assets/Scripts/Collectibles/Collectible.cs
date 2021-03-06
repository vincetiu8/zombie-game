using Interact;
using Networking;
using Photon.Pun;
using UnityEngine;

namespace Collectibles
{
	/// <summary>
	///     Represents an object that is destroyed when the player interacts (collects) it.
	/// </summary>
	public abstract class Collectible : Interactable
	{
		public override void StartInteraction()
		{
			// We pass the player in here for convenience
			Pickup(GameManager.Instance.localPlayerInstance);
			photonView.RPC("RPCInteract", RpcTarget.MasterClient);
		}

		[PunRPC]
		protected void RPCInteract()
		{
			PhotonNetwork.Destroy(gameObject);
		}

		protected abstract void Pickup(GameObject player);
	}
}