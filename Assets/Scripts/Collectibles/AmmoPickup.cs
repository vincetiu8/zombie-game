using System.ComponentModel;
using UnityEngine;
using Weapons;

namespace Collectibles
{
	public class AmmoPickup : Collectible
	{
		#region Methods

		protected override void Pickup(GameObject player)
		{
			AmmoInventory ammoInventory = player.gameObject.GetComponent<AmmoInventory>();
			if (ammoInventory == null) return;

			ammoInventory.DepositAmmo(ammoType, dropAmount);
		}

		#endregion

		#region Variables

		[Header("Ammo Settings")] [Description("The type of ammo being picked up")] [SerializeField]
		private AmmoType ammoType;

		[Description("The amount of ammo to drop")] [SerializeField]
		protected int dropAmount;

		#endregion
	}
}