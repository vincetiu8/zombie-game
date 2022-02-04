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
		[Header("Health Settings")] [SerializeField] [Range(0, 500)]
		protected int initialHealth;

		[SerializeField] private GameObject damagePopupObject;

		protected int Health;

		protected virtual void Awake()
		{
			Health = initialHealth;
		}

		public virtual void ChangeHealth(int change)
		{
			// Can't directly set health because RPCChangeHealth may be overridden
			// We want to ensure we also call it on the client to process changes
			int newHealth = Mathf.Max(Health + change, 0);

			if (newHealth > 0)
			{
				photonView.RPC("RPCChangeHealth", RpcTarget.All, newHealth, change);
				return;
			}

			OnDeath();
		}

		public int GetHealth()
		{
			return Health;
		}

		[PunRPC]
		protected virtual void RPCChangeHealth(int newHealth, int change)
		{
			Health = newHealth;

			SpawnHealthPopup(change);
		}

		protected void SpawnHealthPopup(int change)
		{
			Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),
			                                                    0);

			GameObject popupInstance = Instantiate(damagePopupObject, spawnPos, Quaternion.identity);

			DamagePopup damagePopup = popupInstance.GetComponent<DamagePopup>();
			damagePopup.Setup(change);
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