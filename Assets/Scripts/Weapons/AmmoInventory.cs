using System;
using System.ComponentModel;
using UnityEngine;

namespace Weapons
{
    // AmmoDict is a specific SerializableDictionary
    // This allows the dict to be displayed in the unity editor
    [Serializable]
    public class AmmoDict : SerializableDictionary<AmmoType, AmmoEntry>
    {
    }

    public class AmmoInventory : MonoBehaviour
    {
        [Description("The player's ammo inventory")]
        [SerializeField] private AmmoDict ammoInventory;
        
        private void Awake()
        {
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

        // Get's the amount of bullets of the AmmoType the player currently has
        public int GetAmmo(AmmoType type)
        {
            return ammoInventory[type].currentStock;
        }

        // Deposits ammo into the player's inventory
        // Returns the amount of ammo deposited
        public int DepositAmmo(AmmoType type, int amount)
        {
            AmmoEntry ammoEntry = ammoInventory[type];
            amount = Mathf.Min(amount, ammoEntry.maxCapacity - ammoEntry.currentStock);
            ammoEntry.currentStock += amount;
            ammoInventory[type] = ammoEntry;
            return amount;
        }

        // Withdraws ammo from the player's inventory
        // Returns the amount of ammo withdrawn
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