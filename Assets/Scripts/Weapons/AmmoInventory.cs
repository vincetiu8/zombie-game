using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons 
{    
    public class AmmoInventory : MonoBehaviour
    {
        [SerializeField] List<AmmoEntry> _inventory = new List<AmmoEntry>();

        //returns current ammo stock
        public int GetAmmoStock(Weapons.AmmoType type)
        {
            return _inventory[(int)type].currentStock;
        }

        //sets current stock of type to amount
        public void SetAmmoStock(Weapons.AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            held.currentStock = amount;
            _inventory[(int)type] = held;
        }

        //collect ammo, returns amount collected
        //returns 0 if currentStock = maxCapacity
        public int collectAmmo(Weapons.AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            int collect = Mathf.Min(amount, held.maxCapacity - held.currentStock);
            held.currentStock += collect;
            _inventory[(int)type] = held;
            return collect;
        }
    }
}