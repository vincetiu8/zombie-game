using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public class AmmoDict : SerializableDictionary<AmmoType, AmmoEntry>
    {
    }

    public class AmmoInventory : MonoBehaviour
    {
        [SerializeField] private AmmoDict ammoInventory;

        private void Awake()
        {
            foreach (AmmoType type in Enum.GetValues(typeof(AmmoType)))
            {
                ammoInventory[type] = new AmmoEntry
                {
                    maxCapacity = 0,
                    currentStock = 0
                };
            }
        }

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