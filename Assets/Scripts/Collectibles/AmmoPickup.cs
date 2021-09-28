using System.ComponentModel;
using UnityEngine;
using Weapons;

namespace Collectibles
{
	/// <summary>
	///     Represents a ammo collectible.
	///     Adds ammo to the player's AmmoInventory.
	/// </summary>
	public class AmmoPickup : Collectible
	{
		[Header("Ammo Settings")] [Description("The type of ammo being picked up")] [SerializeField]
		private AmmoType ammoType;

		[Description("The amount of ammo to drop")] [SerializeField]
		protected int dropAmount;

		protected override void Pickup(GameObject player)
		{
			AmmoInventory ammoInventory = player.GetComponent<AmmoInventory>();
			if (ammoInventory == null) return;

			ammoInventory.DepositAmmo(ammoType, dropAmount);
		}
	}
}