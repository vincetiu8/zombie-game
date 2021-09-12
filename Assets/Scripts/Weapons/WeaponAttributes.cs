using System;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public struct WeaponAttributes
    {
        public string description;
        public float damage;
        public int magazineSize;
        public float fireCooldown;
        public float reloadTime;
        public float bulletVelocity;

        public override string ToString()
        {
            return $"Damage: {damage}\n" 
                   + $"Magazine Size: {magazineSize}\n"
                   + $"Fire Cooldown: {fireCooldown}\n"
                   + $"Reload Time: {reloadTime}\n"
                   + $"Bullet Velocity: {bulletVelocity}\n";
        }
    }
}