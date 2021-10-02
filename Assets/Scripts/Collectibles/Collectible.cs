using Interact;
using Networking;
using Photon.Pun;
using UnityEngine;

namespace Collectibles
{
	/// <summary>
	///     Represents an object that is destroyed when the player interacts (collects) it.
	/// </summary>
	public abstract class Collectible : PressInteractable
	{
        protected override void Interact()
		{
			// We pass the player in here for convenience
			Pickup(GameManager.instance.localPlayerInstance);
			photonView.RPC("RPCInteract", RpcTarget.All);
		}

		[PunRPC]
		protected void RPCInteract()
		{
			Destroy(gameObject);
		}

		protected abstract void Pickup(GameObject player);
	}
}