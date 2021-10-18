using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Objects
{
	/// <summary>
	///     Health is the base class for all destructible objects.
	///     Once an object's health reaches 0, it is normally destroyed.
	/// </summary>
	public class HealthController : MonoBehaviourPun 
	{

		public bool canHeal;

		[SerializeField] private int  naturalHealInterval;
		[SerializeField] private int  naturalHealAmount;

		private bool      isHealing;
		private Vector3 position;

		[Header("Health Settings")] [SerializeField] [Range(0, 500)]
		protected int initialHealth;

		protected int Health;

		protected virtual void Awake()
		{
			Health = initialHealth;
			position = gameObject.transform.position;
		}

		private void Update() {
			Debug.Log(canHeal);
			if (this.gameObject.transform.position != position) {
				Debug.Log("Moved");
				canHeal = false;
			}
			CheckNaturalHealing();
			position = gameObject.transform.position;
		}

		private void CheckNaturalHealing() {
			Debug.Log(this.gameObject.tag);
			if (gameObject.tag != "Player" || Health >= initialHealth || !canHeal || Health <= 0 || isHealing) {
				return;
			}

			if (Health + naturalHealAmount > initialHealth) {
				Debug.Log("Healed Over");
				StartCoroutine(NaturalHealing(initialHealth-Health));
				return;
			}
			Debug.Log("OK");
			StartCoroutine(NaturalHealing(naturalHealAmount));
		}

		private IEnumerator NaturalHealing(int healingAmount) {
			isHealing = true;
			yield return new WaitForSeconds(naturalHealInterval);
			ChangeHealth(healingAmount);
			Debug.Log(Health);
			isHealing = false;
		}

		public virtual void ChangeHealth(int change)
		{
			Debug.Log("Changing health!");

			// Can't directly set health because RPCChangeHealth may be overridden
			// We want to ensure we also call it on the client to process changes
			int newHealth = Health + change;

			if (newHealth > 0)
			{
				photonView.RPC("RPCChangeHealth", RpcTarget.All, newHealth);
				return;
			}

			OnDeath();
		}

		public int GetHealth()
		{
			return Health;
		}

		[PunRPC]
		protected virtual void RPCChangeHealth(int newHealth)
		{
			Health = newHealth;
		}

		protected virtual void OnDeath()
		{
			photonView.RPC("RPCOnDeath", photonView.Owner);
		}

		// Called on master as only master can destroy 
		[PunRPC]
		protected virtual void RPCOnDeath()
		{
			PhotonNetwork.Destroy(gameObject);
		}
	}
}