using Networking;
using PlayerScripts;
using UnityEngine;
using Weapons;

namespace Shop
{
	/// <summary>
	///     Handles ammo machine interactions
	/// </summary>
	public class AmmoMachine : ItemShop
	{
		[Header("Ammo Settings")] [SerializeField]
		private AmmoType ammoType;

		[SerializeField] private int ammoAmount;

		protected override void OnPurchase()
		{
			AmmoInventory ammoInventory = GameManager.Instance.localPlayerInstance.GetComponent<AmmoInventory>();
			if (ammoInventory == null) return;

			ammoInventory.DepositAmmo(ammoType, ammoAmount);
		}
	}
}