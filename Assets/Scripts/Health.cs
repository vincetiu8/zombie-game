using Photon.Pun;
using UnityEngine;

/// <summary>
///     Health is the base class for all destructible objects.
///     Once an object's health reaches 0, it is normally destroyed.
/// </summary>
public class Health : MonoBehaviourPun
{
	[SerializeField] protected int initialHealth;

	protected int health;

	private void Awake()
	{
		health = initialHealth;
	}

	public int GetHealth()
	{
		return health;
	}

	public int GetRoundedHealth()
	{
		return Mathf.RoundToInt(health);
	}

	public virtual void ChangeHealth(int change)
	{
		health += change;

		if (health > 0) return;

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