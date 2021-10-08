using Photon.Pun;
using UnityEngine;

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
			photonView.RPC("RPCChangeHealth", RpcTarget.All, newHealth);
			return;
		}

		OnDeath();
	}

	[PunRPC]
	protected virtual void RPCChangeHealth(int newHealth)
	{
		Debug.Log(newHealth);
		Health = newHealth;
	}

	protected virtual void OnDeath()
	{
		PhotonNetwork.Destroy(gameObject);
	}
}