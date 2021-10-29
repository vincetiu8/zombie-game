using System.Collections;
using Networking;
using Objects;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
	public class PlayerHealth : AnimatedHealth
	{
		[Header("Player Death Settings")] [SerializeField]
		private Transform cameraObject;

        [SerializeField] private GameObject reviveInteract;

		[SerializeField] private GameObject weapons;

		[Header("Natural Healing Settings")] [SerializeField]
		private int healAmount;

		[SerializeField] private float maxHealDelay;

		[SerializeField] private GameObject[] childrenToChangeTagOnDeath;
		private                  float        _carryHealth;
		private                  float        _healDelay;

		private PlayerInteract _playerInteract;
        private PlayerInput _playerInput;
        [SerializeField] private Sprite playerDownSprite;
        [SerializeField] private bool playerDown;
        
        [SerializeField]
        private GameObject[] objectsToDisableOnDown;

		protected override void Start()
		{
			base.Start();
			_playerInteract = GetComponent<PlayerInteract>();
            _playerInput = GetComponent<PlayerInput>();
        }

		private void Update()
		{
			if (!photonView.IsMine || Health >= initialHealth || Health <= 0) return;

			if (_healDelay > 0)
			{
				_healDelay -= Time.deltaTime;
				return;
			}

			_carryHealth += healAmount * Time.deltaTime;
			int intHealth = (int)_carryHealth;

			if (intHealth != 0) ChangeHealth(intHealth);

			_carryHealth -= intHealth;
		}

		public override void ChangeHealth(int change)
		{
			int maxChange = initialHealth - Health;
			base.ChangeHealth(change > maxChange ? maxChange : change);
		}

		[PunRPC]
		protected override void RPCChangeHealth(int newHealth, int change)
		{
			base.RPCChangeHealth(newHealth, change);
			if (!photonView.IsMine || change >= 0) return;
			_playerInteract.CancelInteraction();
			ResetNaturalHealing();
		}

		public void ResetNaturalHealing()
		{
			_healDelay = maxHealDelay;
			_carryHealth = 0;
		}

        protected override void OnDeath()
        {
            //if (!photonView.IsMine) return;
            Debug.Log("player is down");
            photonView.RPC("RPCPlayerDown", RpcTarget.All);
        }

        private IEnumerator DownedTimer(float downTime)
        {
            yield return new WaitForSeconds(downTime);
            photonView.RPC("RPCInitialOnDeath", RpcTarget.All);
        }

        public void ReviveSuccessful()
        {
            photonView.RPC("RPCReviveSuccessful", RpcTarget.All);
        }



        [PunRPC]
        private void RPCReviveSuccessful()
        {
            Debug.Log("revive successful");
            StopCoroutine(DownedTimer(10));
            reviveInteract.SetActive(false);
            _playerInput.currentActionMap.Enable();
            playerDown = false;
            foreach (GameObject gameObject in objectsToDisableOnDown) gameObject.SetActive(true);
        }

        [PunRPC]
        private void RPCPlayerDown()
        {
            Debug.Log("RPC called");
            reviveInteract.SetActive(true);
            StartCoroutine(DownedTimer(10));
            spriteRenderer.sprite = playerDownSprite;
            _playerInput.currentActionMap.Disable();
            playerDown = true;
            foreach (GameObject gameObject in objectsToDisableOnDown) gameObject.SetActive(false);
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

			GameManager.Instance.spectatorManager.OnPlayerDeath(photonView.Owner.GetPlayerNumber());

			if (!photonView.IsMine) return;

			Debug.Log(photonView.IsMine);
			GameManager.Instance.spectatorManager.enabled = true;
			base.RPCOnDeath();
		}
	}
}