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

	protected virtual void Awake()
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

	public virtual void ChangeHealth(int change)
	{
		Health += change;

		if (Health > 0) return;

		OnDeath();
	}

	protected virtual void OnDeath()
	{
		photonView.RPC("RPCOnDeath", RpcTarget.All);
	}

	[PunRPC]
	protected void RPCOnDeath()
	{
		Destroy(gameObject);
	}
}