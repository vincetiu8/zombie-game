using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons 
{
    [System.Serializable]
    public struct AmmoEntry
    {
        public string name;
        //name field included for editing convenience, not essential
        public int maxCapacity;
        public int currentStock;
    }
    
    public class AmmoInventory : MonoBehaviour
    {
        [SerializeField] List<AmmoEntry> _inventory = new List<AmmoEntry>();

        //returns current ammo stock
        public int getAmmoStock(Weapons.AmmoType type)
        {
            return _inventory[(int)type].currentStock;
        }

        //collect ammo, returns amount collected
        public int collectAmmo(Weapons.AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            int collect = Mathf.Min(amount, held.currentStock);
            held.currentStock += collect;
            _inventory[(int)type] = held;
            return collect;
        }

        //spend ammo, returns amount spent
        public int spendAmmo(Weapons.AmmoType type, int amount)
        {
            AmmoEntry held = _inventory[(int)type];
            int spend = Mathf.Min(amount, held.currentStock);
            held.currentStock -= spend;
            _inventory[(int)type] = held;
            return spend;
        }

        //Ensure _inventory always matches the AmmoType enum in case of code changes
        private void Reset() { OnValidate(); }
        private void OnValidate() {
            var ammoNames = System.Enum.GetNames(typeof(Weapons.AmmoType));
            var inventory = new List<AmmoEntry>(ammoNames.Length);        
            for(int i = 0; i < ammoNames.Length; i++) {
                var existing = _inventory.Find(
                    (entry) => { return entry.name == ammoNames[i]; });
                existing.name = ammoNames[i];
                existing.currentStock = Mathf.Min(existing.currentStock, existing.maxCapacity);
                inventory.Add(existing);
            }
            _inventory = inventory;
        }
    }
}