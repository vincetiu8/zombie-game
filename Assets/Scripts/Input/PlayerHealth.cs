using Interact;
using Networking;
using Objects;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Input
{
	public class PlayerHealth : AnimatedHealth
	{
		[Header("Player Death Settings")] [SerializeField]
		private Transform cameraObject;

		[SerializeField] private GameObject weapons;

		private PlayerInteract _playerInteract;

		protected override void Start()
		{
			base.Start();
			_playerInteract = GetComponent<PlayerInteract>();
		}

		// Makes it so that taking damaged also cancels current input 
		public override void ChangeHealth(int change)
		{
			if (change < 0) _playerInteract.CancelInteraction();
			base.ChangeHealth(change);
		}

		[PunRPC]
		protected override void RPCInitialOnDeath(PhotonMessageInfo info)
		{
			GameManager.Instance.RemovePlayerInstance(photonView.Owner.GetPlayerNumber());
			weapons.SetActive(false);

			if (GameManager.Instance.PlayerInstances.Count == 0) cameraObject.parent = null;

			base.RPCInitialOnDeath(info);
		}

		protected override void RPCOnDeath()
		{
			if (photonView.IsMine)
			{
				GameManager.Instance.spectatorManager.enabled = true;
				base.RPCOnDeath();
			}

			GameManager.Instance.spectatorManager.OnPlayerDeath(PhotonNetwork.LocalPlayer.GetPlayerNumber());
		}
	}
}