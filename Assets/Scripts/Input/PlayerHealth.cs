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

		public float _healDelay;

		[Header("Player Death Settings")] [SerializeField]
		private Transform cameraObject;

		[SerializeField] private GameObject weapons; 
		
		[SerializeField] private int healAmount;

		[SerializeField] private float maxHealDelay;

		private PlayerInteract _playerInteract;
		private float            _carryHealth;


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

		public void ResetNaturalHealing() {
			if (Health >= initialHealth || Health <= 0) return;

			_healDelay = maxHealDelay;
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