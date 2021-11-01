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

		protected int Health;

		protected virtual void Awake()
		{
			Health = initialHealth;
		}

		public virtual void ChangeHealth(int change)
		{
			// Can't directly set health because RPCChangeHealth may be overridden
			// We want to ensure we also call it on the client to process changes
			int newHealth = Health + change;

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
			Debug.Log(Health);
			Debug.Log(newHealth);
			Debug.Log(change);
			Health = newHealth;
		}

		protected virtual void OnDeath()
		{
			photonView.RPC("RPCOnDeath", photonView.Owner);
		}

		// Called on master as only master can destroy 
		[PunRPC]
		protected virtual void RPCOnDeath(PhotonMessageInfo info)
		{
			PhotonNetwork.Destroy(gameObject);
		}
	}
}