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
		[SerializeField] private Vector3 offset = new Vector3(0.4f, -0.55f, 0);

		protected override void Start()
		{
			base.Start();
			offset = transform.localPosition;
		}

		public override void StartInteraction()
		{
			if (GameManager.Instance.localPlayerInstance.GetComponent<WeaponsHandler>().CheckIfWeaponAlreadyExists
				    (gameObject)) return;
			base.StartInteraction();
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
			transform.localPosition = offset;
			transform.localRotation = Quaternion.identity;
			GetComponent<Collider2D>().enabled = false;
			foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.sortingLayerID = SortingLayer.NameToID("Players");
				spriteRenderer.sortingOrder = 2;
			}

			player.GetComponent<WeaponsHandler>().AddWeapon(gameObject);
			gameObject.SetActive(false);
		}

		public void PickupWeapon()
		{
			photonView.RPC("RPCPickupWeapon", RpcTarget.All);
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