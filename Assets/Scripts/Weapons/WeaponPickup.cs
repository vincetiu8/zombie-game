using Interact;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Weapons
{
	public class WeaponPickup : Interactable
	{
		private Vector3        _offset;
		private WeaponsHandler _weaponsHandler;

		public override void Interact()
		{
			photonView.RPC("RPCPickupWeapon", RpcTarget.All);
		}

		[PunRPC]
		private void RPCPickupWeapon(PhotonMessageInfo info)
		{
			int playerNumber = info.Sender.GetPlayerNumber();
			GameObject player = GameManager.instance.playerInstances[playerNumber];
			transform.parent = player.transform.Find("PlayerObject").Find("Weapons");
			transform.localPosition = _offset;
			GetComponent<Collider2D>().enabled = false;
			foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.sortingLayerID = SortingLayer.NameToID("Actors");
				spriteRenderer.sortingOrder = 2;
			}

			_weaponsHandler = player.GetComponent<WeaponsHandler>();
			_weaponsHandler.AddWeapon(gameObject);
		}

		public void DropWeapon()
		{
			photonView.RPC("RPCDropWeapon", RpcTarget.All);
		}

		[PunRPC]
		private void RPCDropWeapon(PhotonMessageInfo info)
		{
			_offset = transform.localPosition;
			transform.parent = null;
			GetComponent<Collider2D>().enabled = true;
			foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.sortingLayerID = SortingLayer.NameToID("Objects");
				spriteRenderer.sortingOrder = 1;
			}
		}
	}
}