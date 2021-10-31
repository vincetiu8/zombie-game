using Enemy;
using Networking;
using Objects;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace PlayerScripts
{
	public class PlayerHealth : AnimatedHealth
	{
		[Header("Player Death Settings")] [SerializeField]
		private Transform cameraObject;

		[SerializeField] private GameObject weapons;

		[Header("Natural Healing Settings")] [SerializeField]
		private int healAmount;

		[SerializeField] private float maxHealDelay;
		private                  float _carryHealth;
		private                  float _healDelay;

		private PlayerInteract _playerInteract;

        [SerializeField] private GameObject[] childrenToChangeTagOnDeath;

        protected override void Start()
		{
			base.Start();
			_playerInteract = GetComponent<PlayerInteract>();
		}

		private void Update()
		{
            if (Health >= initialHealth || Health <= 0) return;

			if (_healDelay > 0)
			{
				_healDelay -= Time.deltaTime;
				return;
			}

			_carryHealth += healAmount * Time.deltaTime;
			int intHealth = (int)_carryHealth;

			ChangeHealth(intHealth);

			_carryHealth -= intHealth;
		}

		// Makes it so that taking damaged also cancels current input 
		public override void ChangeHealth(int change)
		{
			if (change < 0)
			{
				_playerInteract.CancelInteraction();
				ResetNaturalHealing();
			}

			base.ChangeHealth(change);
		}

		public void ResetNaturalHealing()
		{
			if (Health >= initialHealth || Health <= 0) return;

			_healDelay = maxHealDelay;
			_carryHealth = 0;
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
            foreach (GameObject obj in childrenToChangeTagOnDeath)
            {
                obj.tag = "DeadPlayer";
            }

            GameManager.Instance.spectatorManager.OnPlayerDeath(PhotonNetwork.LocalPlayer.GetPlayerNumber());

            if (!photonView.IsMine) return;
            Debug.Log("Destroying player object");
            GameManager.Instance.spectatorManager.enabled = true;
            base.RPCOnDeath();

        }
	}
}