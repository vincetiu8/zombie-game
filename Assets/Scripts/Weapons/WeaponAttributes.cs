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
        public GameObject bulletPrefab;

        public override string ToString()
        {
            return $"Magazine Size: {magazineSize}\n"
                   + $"Fire Cooldown: {fireCooldown}\n"
                   + $"Reload Time: {reloadTime}\n"
                   + $"Bullet Velocity: {bulletVelocity}\n";
        }
    }
}