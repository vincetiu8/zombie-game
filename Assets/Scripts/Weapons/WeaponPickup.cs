using Interact;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Weapons
{
	public class WeaponPickup : Interactable
	{
		public Vector3 offset;

		public override void Interact()
		{
			photonView.RPC("RPCPickup", RpcTarget.All);
		}

		[PunRPC]
		private void RPCPickup(PhotonMessageInfo info)
		{
			int playerNumber = info.Sender.GetPlayerNumber();
			GameObject player = GameManager.instance.playerInstances[playerNumber];
			transform.parent = player.transform.Find("Weapons");
			transform.localPosition = offset;
			GetComponent<Collider2D>().enabled = false;
			player.GetComponent<WeaponsHandler>().AddWeapon(gameObject);
		}
	}
}