using System;
using System.ComponentModel;
using UnityEngine;
using Weapons;

namespace PlayerScripts
{
	[Serializable]
	public class AmmoDict : SerializableDictionary<AmmoType, AmmoEntry>
	{
	}

	public class AmmoInventory : MonoBehaviour
	{
		[Description("The player's ammo inventory")] [SerializeField]
		private AmmoDict ammoInventory;

		private void Awake()
		{
			ammoInventory ??= new AmmoDict();

			// Loops through all ammo types and ensures they have a corresponding entry in the dict
			foreach (AmmoType type in Enum.GetValues(typeof(AmmoType)))
			{
				// Do nothing if ammo type is already set in the inventory
				if (ammoInventory.ContainsKey(type)) continue;

				// Create an empty entry
				ammoInventory[type] = new AmmoEntry
				                      {
					                      maxCapacity = 0,
					                      currentStock = 0
				                      };
			}
		}

		/// <summary>
		///     Gets the amount of bullets of the AmmoType the player currently has.
		/// </summary>
		/// <param name="type">The ammo type to search for</param>
		/// <returns>The amount of bullets the player has of the type</returns>
		public int GetAmmo(AmmoType type)
		{
			return ammoInventory[type].currentStock;
		}

		/// <summary>
		///     Deposits ammo into the player's inventory.
		/// </summary>
		/// <param name="type">The type of ammo to deposit</param>
		/// <param name="amount">The amount of ammo to deposit</param>
		/// <returns>The amount of ammo actually deposited</returns>
		public int DepositAmmo(AmmoType type, int amount)
		{
			AmmoEntry ammoEntry = ammoInventory[type];
			amount = Mathf.Min(amount, ammoEntry.maxCapacity - ammoEntry.currentStock);
			ammoEntry.currentStock += amount;
			ammoInventory[type] = ammoEntry;
			return amount;
		}

		/// <summary>
		///     Withdraws ammo from the player's inventory.
		/// </summary>
		/// <param name="type">The type of ammo to withdraw</param>
		/// <param name="amount">The amount of ammo to withdraw</param>
		/// <returns>The amount of ammo actually withdrawn</returns>
		public int WithdrawAmmo(AmmoType type, int amount)
		{
			AmmoEntry ammoEntry = ammoInventory[type];
			amount = Mathf.Min(amount, ammoEntry.currentStock);
			ammoEntry.currentStock -= amount;
			ammoInventory[type] = ammoEntry;
			return amount;
		}
	}
}