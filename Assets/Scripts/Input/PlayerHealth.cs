using System;
using System.Collections;
using Interact;
using Networking;
using Objects;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Input
{
	public class PlayerHealth : AnimatedHealth
	{
		[Header("Player Death Settings")] [SerializeField]
		private Transform cameraObject;

		[SerializeField] private GameObject weapons; 
		
		[Header("Natural Healing Settings")]
		[SerializeField] private int healAmount;
		[SerializeField] private float maxHealDelay;

		private PlayerInteract _playerInteract;
		private float          _carryHealth;
		private float          _healDelay;


		protected override void Start()
		{
			base.Start();
			_playerInteract = GetComponent<PlayerInteract>();
		}

		// Makes it so that taking damaged also cancels current input 
		public override void ChangeHealth(int change)
		{
			if (change < 0) {
				_playerInteract.CancelInteraction();
				ResetNaturalHealing();
			}
			base.ChangeHealth(change);
		}

		public void ResetNaturalHealing() {
			if (Health >= initialHealth || Health <= 0) return;

			_healDelay = maxHealDelay;  
			_carryHealth = 0;
		}

		private void Update() {
			if (Health >= initialHealth || Health <= 0) return;

			if (_healDelay > 0) {
				_healDelay -= Time.deltaTime;
				return;
			}

			_carryHealth += healAmount * Time.deltaTime;
			int intHealth = (int)_carryHealth;

			ChangeHealth(intHealth);

			_carryHealth -= intHealth;

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