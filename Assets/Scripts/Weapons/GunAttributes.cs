using System;
using System.ComponentModel;
using UnityEngine;

namespace Weapons
{
    // AmmoType represents all the ammo types in the game
    // Each gun also has an AmmoType, representing the bullets it uses
    public enum AmmoType { Primary, Heavy, Special }

    // An entry in the AmmoInventory
    // Each AmmoType has exactly 1 associated AmmoEntry
    [Serializable]
    public struct AmmoEntry
    {
        [Description("The maximum number of bullets of this type the player can hold")]
        public int maxCapacity;
        
        [Description("The current number of bullets of this kind the player has")]
        public int currentStock;
    }
    
    // Extra features a Gun has in addition to a normal weapon
    [Serializable]
    public class GunAttributes : WeaponAttributes
    {
        [Description("The number of bullets that can be in the gun at once")]
        public int magazineSize;
        
        [Description("The time the gun takes to reload")]
        public float reloadTime;
        
        [Description("The speed of the bullets")]
        public float bulletSpeed;

        public override string ToString()
        {
            return base.ToString()
                   + $"Magazine Size: {magazineSize}\n"
                   + $"Reload Time: {reloadTime}\n"
                   + $"bulletVelocity: {bulletSpeed}\n";
        }
    }
}