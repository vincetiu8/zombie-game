using Photon.Pun;
using UnityEngine;

/// <summary>
///     Health is the base class for all destructible objects.
///     Once an object's health reaches 0, it is normally destroyed.
/// </summary>
public class HealthController : MonoBehaviourPun
{
	[SerializeField] protected int initialHealth;

	protected int Health;

	private void Awake()
	{
		Health = initialHealth;
	}

	public int GetHealth()
	{
		return Health;
	}

	public int GetRoundedHealth()
	{
		return Mathf.RoundToInt(Health);
	}

	public void ChangeHealth(int change)
	{
		photonView.RPC("RPCChangeHealth", RpcTarget.All, change);
	}

	[PunRPC]
	protected virtual void RPCChangeHealth(int change)
	{
		Health += change;

		if (Health > 0) return;

		OnDeath();
	}

	protected virtual void OnDeath()
	{
		photonView.RPC("RpcOnDeath", RpcTarget.All);
	}

	[PunRPC]
	protected void RpcOnDeath()
	{
		Destroy(gameObject);
	}
}