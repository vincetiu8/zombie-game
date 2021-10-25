using Interact;
using Networking;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using PlayerScripts;
using UnityEngine;

namespace Weapons
{
	public class WeaponPickup : TimedInteractable
	{
		private Vector3        _offset;
		private WeaponsHandler _weaponsHandler;

		protected override void Start()
		{
			base.Start();
			_offset = transform.localPosition;
		}

		protected override void OnSuccessfulInteraction()
		{
			photonView.RPC("RPCPickupWeapon", RpcTarget.All);
			FinishInteraction();
		}

		[PunRPC]
		private void RPCPickupWeapon(PhotonMessageInfo info)
		{
			int playerNumber = info.Sender.GetPlayerNumber();
			GameObject player = GameManager.Instance.PlayerInstances[playerNumber];
			transform.parent = player.transform.Find("PlayerObject").Find("Weapons");
			transform.localPosition = _offset;
			transform.localRotation = Quaternion.identity;
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