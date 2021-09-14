using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons 
{    
    public class AmmoInventory : MonoBehaviour
    {
        [SerializeField] AmmoEntry[] _inventory = new AmmoEntry[3];

        //returns current ammo stock
        public int GetAmmoStock(AmmoType type)
        {
            return _inventory[(int)type].currentStock;
        }

        //spend ammo, returns amount spent
        //returns 0 if currentStock of AmmoType = 0 (i.e no stock of AmmoType)
        public int SpendAmmo(AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            int spend = Mathf.Min(amount, held.currentStock);
            held.currentStock -= spend;
            _inventory[(int)type] = held;
            return spend;
        }

        //collect ammo, returns amount collected
        //returns 0 if currentStock = maxCapacity
        public int CollectAmmo(AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            int collect = Mathf.Min(amount, held.maxCapacity - held.currentStock);
            held.currentStock += collect;
            _inventory[(int)type] = held;
            return collect;
        }
    }
}